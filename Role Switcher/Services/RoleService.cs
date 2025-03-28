using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using XrmToolBox.Extensibility;

namespace Role_Switcher.Services
{
    /// <summary>
    /// Provides services for managing and applying security roles to users within Dynamics 365.
    /// </summary>
    public class RoleService
    {
        private readonly IOrganizationService _service;
        private readonly RSLogManager _logger;
        private readonly Action<WorkAsyncInfo> _workAsync;
        private readonly Action<string> _setWorkingMessage;

        private List<Guid> _usersToEdit;
        private List<string> _rolesToApply;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="service">Dynamics 365 organization service instance.</param>
        /// <param name="logger">Logger for recording log messages.</param>
        /// <param name="workAsync">Delegate to run asynchronous operations.</param>
        /// <param name="setWorkingMessage">Delegate to update UI with a working message.</param>
        public RoleService(IOrganizationService service, RSLogManager logger, Action<WorkAsyncInfo> workAsync, Action<string> setWorkingMessage)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _workAsync = workAsync ?? throw new ArgumentNullException(nameof(workAsync));
            _setWorkingMessage = setWorkingMessage ?? throw new ArgumentNullException(nameof(setWorkingMessage));
        }

        /// <summary>
        /// Retrieves all role names from the default Business Unit.
        /// </summary>
        /// <param name="callback">Callback to handle the list of role names.</param>
        public void FetchAllRoleNamesFromDefaultBU(Action<List<string>> callback)
        {
            _workAsync(new WorkAsyncInfo
            {
                Message = "Fetching all roles...",
                Work = (_, args) =>
                {
                    var defaultBu = _service.RetrieveMultiple(new QueryExpression("businessunit")
                    {
                        ColumnSet = new ColumnSet("businessunitid"),
                        Criteria = new FilterExpression
                        {
                            Conditions = { new ConditionExpression("parentbusinessunitid", ConditionOperator.Null) }
                        }
                    }).Entities.First().Id;

                    var roles = _service.RetrieveMultiple(new QueryExpression("role")
                    {
                        ColumnSet = new ColumnSet("name"),
                        Criteria = new FilterExpression
                        {
                            Conditions = { new ConditionExpression("businessunitid", ConditionOperator.Equal, defaultBu) }
                        }
                    });

                    args.Result = roles.Entities
                        .Select(e => e.GetAttributeValue<string>("name"))
                        .Where(n => !string.IsNullOrEmpty(n))
                        .ToList();
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger.Log(LogLevel.Error, args.Error.ToString());
                        return;
                    }
                    var roles = args.Result as List<string>;
                    _logger.Log(LogLevel.Information, $"Found {roles?.Count ?? 0} roles");
                    callback?.Invoke(roles);
                }
            });
        }

        /// <summary>
        /// Applies roles to a set of users.
        /// </summary>
        /// <param name="usersToEdit">List of user GUIDs to apply roles to.</param>
        /// <param name="rolesToApply">List of role names to assign.</param>
        /// <param name="replaceRoles">True to replace existing roles, false to append.</param>
        public void ApplyRolesToUsers(List<Guid> usersToEdit, List<string> rolesToApply, bool replaceRoles)
        {
            _usersToEdit = usersToEdit;
            _rolesToApply = rolesToApply;
            GetRolesForBUs(replaceRoles);
        }

        /// <summary>
        /// Initiates the retrieval of applicable roles from each user's Business Unit.
        /// </summary>
        private void GetRolesForBUs(bool replaceRoles) => _workAsync(new WorkAsyncInfo
        {
            Message = "Fetching roles...",
            Work = (_, args) => args.Result = _rolesToApply.Count == 0 ? new EntityCollection() : RetrieveRoles(),
            PostWorkCallBack = args => HandleRoleRetrievalResponse(args, replaceRoles)
        });

        /// <summary>
        /// Retrieves the roles matching the names in _rolesToApply.
        /// </summary>
        private EntityCollection RetrieveRoles()
        {
            var query = new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("roleid", "businessunitid")
            };

            var filter = new FilterExpression(LogicalOperator.Or);
            foreach (var roleName in _rolesToApply)
            {
                filter.AddCondition("name", ConditionOperator.Equal, roleName);
            }
            query.Criteria.AddFilter(filter);

            return _service.RetrieveMultiple(query);
        }

        /// <summary>
        /// Processes the result of the role query and initiates user updates.
        /// </summary>
        private void HandleRoleRetrievalResponse(RunWorkerCompletedEventArgs args, bool replaceRoles)
        {
            if (args.Error != null)
            {
                _logger.Log(LogLevel.Error, args.Error.ToString());
                return;
            }

            var result = args.Result as EntityCollection;
            var buRoles = result?.Entities.Select(e => new Tuple<Guid, string, Guid>(
                e.GetAttributeValue<EntityReference>("businessunitid").Id,
                e.GetAttributeValue<string>("name"),
                e.Id)).ToList() ?? new List<Tuple<Guid, string, Guid>>();

            _logger.Log(LogLevel.Information, $"Found {result?.Entities.Count ?? 0} roles");
            UpdateUsersRoles(buRoles, replaceRoles);
        }

        /// <summary>
        /// Triggers the asynchronous role assignment for users.
        /// </summary>
        private void UpdateUsersRoles(List<Tuple<Guid, string, Guid>> buRoles, bool replaceRoles)
        {
            _workAsync(new WorkAsyncInfo
            {
                Message = "Assigning Roles...",
                Work = (worker, _) =>
                {
                    worker.WorkerReportsProgress = true;
                    AssignRolesToUsers(worker, buRoles, replaceRoles);
                },
                ProgressChanged = HandleProgressChanged,
                PostWorkCallBack = HandlePostWorkCallback
            });
        }

        /// <summary>
        /// Assigns roles to each user while reporting progress.
        /// </summary>
        private void AssignRolesToUsers(BackgroundWorker worker, List<Tuple<Guid, string, Guid>> buRoles, bool replaceRoles)
        {
            worker.ReportProgress(0, CreateProgressData("Assigning Roles...", "0% Done...", null));
            var grouped = QueryUsersAndGroup(replaceRoles);
            int usersDone = 0;

            foreach (var group in grouped)
            {
                usersDone++;
                var userId = group.First().Id;
                var userBuId = group.First().GetAttributeValue<EntityReference>("businessunitid").Id;
                var rolesToAdd = buRoles.Where(r => r.Item1 == userBuId).ToList();

                ExecuteRoleUpdates(userId, rolesToAdd, group, replaceRoles);

                int progress = (int)Math.Ceiling((float)usersDone / grouped.Count() * 100);
                worker.ReportProgress(progress, CreateProgressData($"{progress}% Done...", group.Key, null));
            }
        }

        /// <summary>
        /// Queries user data and groups by user identifier.
        /// </summary>
        private IEnumerable<IGrouping<string, Entity>> QueryUsersAndGroup(bool replaceRoles)
        {
            var query = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("fullname", "systemuserid", "businessunitid")
            };

            var filter = new FilterExpression(LogicalOperator.Or);
            foreach (var userId in _usersToEdit)
            {
                filter.AddCondition("systemuserid", ConditionOperator.Equal, userId);
            }
            query.Criteria.AddFilter(filter);

            if (replaceRoles)
            {
                var roleMembership = query.AddLink("systemuserroles", "systemuserid", "systemuserid", JoinOperator.LeftOuter);
                roleMembership.EntityAlias = "rolemembership";

                var role = roleMembership.AddLink("role", "roleid", "roleid", JoinOperator.LeftOuter);
                role.EntityAlias = "role";
                role.Columns.AddColumn("roleid");
            }

            var results = _service.RetrieveMultiple(query);
            return results.Entities.GroupBy(e => $"{e.GetAttributeValue<string>("fullname")}:{e.Id}");
        }

        /// <summary>
        /// Executes associate and disassociate requests to update user roles.
        /// </summary>
        private void ExecuteRoleUpdates(Guid userId, List<Tuple<Guid, string, Guid>> rolesToAdd, IGrouping<string, Entity> group, bool replaceRoles)
        {
            var associateRequests = CreateAssociateRequests(userId, rolesToAdd);
            var disassociateRequests = CreateDisassociateRequests(userId, group, replaceRoles);

            _service.Execute(new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true },
                Requests = disassociateRequests
            });

            _service.Execute(new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true },
                Requests = associateRequests
            });
        }

        /// <summary>
        /// Constructs associate requests to link users with roles.
        /// </summary>
        private OrganizationRequestCollection CreateAssociateRequests(Guid userId, List<Tuple<Guid, string, Guid>> rolesToAdd)
        {
            var requests = new OrganizationRequestCollection();
            foreach (var role in rolesToAdd)
            {
                requests.Add(new AssociateRequest
                {
                    Target = new EntityReference("systemuser", userId),
                    RelatedEntities = new EntityReferenceCollection { new EntityReference("systemrole", role.Item3) },
                    Relationship = new Relationship("systemuserroles_association")
                });
            }
            return requests;
        }

        /// <summary>
        /// Constructs disassociate requests to unlink users from roles.
        /// </summary>
        private OrganizationRequestCollection CreateDisassociateRequests(Guid userId, IGrouping<string, Entity> group, bool replaceRoles)
        {
            var requests = new OrganizationRequestCollection();
            if (!replaceRoles) return requests;

            foreach (var role in group)
            {
                if (role.TryGetAttributeValue("role.roleid", out AliasedValue roleId))
                {
                    requests.Add(new DisassociateRequest
                    {
                        Target = new EntityReference("systemuser", userId),
                        RelatedEntities = new EntityReferenceCollection { new EntityReference("systemrole", (Guid)roleId.Value) },
                        Relationship = new Relationship("systemuserroles_association")
                    });
                }
            }
            return requests;
        }

        /// <summary>
        /// Creates a tuple with progress data for use in reporting.
        /// </summary>
        private Tuple<string, string, string> CreateProgressData(string message, string status, string error)
        {
            return new Tuple<string, string, string>(message, status, error);
        }

        /// <summary>
        /// Handles progress updates during role assignment.
        /// </summary>
        private void HandleProgressChanged(ProgressChangedEventArgs args)
        {
            var data = (Tuple<string, string, string>)args.UserState;
            _setWorkingMessage?.Invoke(data.Item1);
            _logger.Log(LogLevel.Information, $"Applied roles to {data.Item2}, {data.Item1}");
            if (data.Item3 != null) _logger.Log(LogLevel.Error, data.Item3);
        }

        /// <summary>
        /// Called after all role assignment work completes.
        /// </summary>
        private void HandlePostWorkCallback(RunWorkerCompletedEventArgs args)
        {
            _usersToEdit.Clear();
            if (args.Error != null)
            {
                _logger.Log(LogLevel.Error, args.Error.ToString());
            }
            else if (args.Result is EntityCollection result)
            {
                _logger.Log(LogLevel.Information, $"Found {result.Entities.Count} roles");
            }
        }
    }
}