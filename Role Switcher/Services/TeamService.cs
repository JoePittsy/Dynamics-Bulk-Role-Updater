using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XrmToolBox.Extensibility;

namespace Role_Switcher.Services
{
    /// <summary>
    /// Provides services for retrieving and managing team memberships within Dynamics 365.
    /// </summary>
    public class TeamsService
    {
        private readonly IOrganizationService _service;
        private readonly RSLogManager _logger;
        private readonly Action<WorkAsyncInfo> _workAsync;
        private readonly Action<string> _setWorkingMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsService"/> class.
        /// </summary>
        /// <param name="service">Dynamics 365 organization service instance.</param>
        /// <param name="logger">Logger for recording log messages.</param>
        /// <param name="workAsync">Delegate to run asynchronous operations.</param>
        /// <param name="setWorkingMessage">Delegate to update UI with a working message.</param>
        public TeamsService(IOrganizationService service, RSLogManager logger, Action<WorkAsyncInfo> workAsync, Action<string> setWorkingMessage)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _workAsync = workAsync ?? throw new ArgumentNullException(nameof(workAsync));
            _setWorkingMessage = setWorkingMessage ?? throw new ArgumentNullException(nameof(setWorkingMessage));
        }

        public void ApplyTeamsToUsers(List<Guid> usersToEdit, List<string> teamsToApply, bool replaceTeams)
        {
            if (usersToEdit == null || usersToEdit.Count == 0 || teamsToApply == null || teamsToApply.Count == 0)
            {
                _logger.Log(LogLevel.Warning, "No users or teams provided for ApplyTeamsToUsers.");
                return;
            }

            _workAsync(new WorkAsyncInfo
            {
                Message = "Applying teams to users...",
                Work = (_, args) =>
                {
                    // Step 1: Fetch target teams by name
                    var teamQuery = new QueryExpression("team")
                    {
                        ColumnSet = new ColumnSet("teamid", "name"),
                        Criteria = new FilterExpression(LogicalOperator.Or)
                    };
                    foreach (var name in teamsToApply)
                    {
                        teamQuery.Criteria.Conditions.Add(new ConditionExpression("name", ConditionOperator.Equal, name));
                    }

                    var teamEntities = _service.RetrieveMultiple(teamQuery).Entities;
                    if (!teamEntities.Any())
                        throw new InvalidOperationException("None of the specified teams were found.");

                    var teamMap = teamEntities.ToDictionary(
                        e => e.GetAttributeValue<string>("name"),
                        e => e.Id
                    );

                    // Step 2: Preload team memberships if replacing
                    var teamMemberships = new Dictionary<Guid, HashSet<Guid>>();
                    if (replaceTeams)
                    {
                        foreach (var teamId in teamMap.Values)
                        {
                            var members = GetTeamMembers(teamId);
                            teamMemberships[teamId] = new HashSet<Guid>(members);
                        }
                    }

                    // Step 3: Apply logic
                    foreach (var userId in usersToEdit)
                    {
                        foreach (var teamId in teamMap.Values)
                        {
                            bool userIsInTeam = replaceTeams && teamMemberships.TryGetValue(teamId, out var membersInTeam) && membersInTeam.Contains(userId);

                            if (replaceTeams && userIsInTeam)
                            {
                                RemoveUserFromTeam(userId, teamId);
                            }

                            if (!userIsInTeam)
                            {
                                AddUserToTeam(userId, teamId);
                            }
                        }
                    }

                    args.Result = usersToEdit.Count;
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                        _logger.Log(LogLevel.Error, $"Error applying teams: {args.Error}");
                    else
                        _logger.Log(LogLevel.Information, $"Successfully updated teams for {args.Result} users.");
                }
            });
        }

        /// <summary>
        /// Retrieves all teams in the system.
        /// </summary>
        /// <param name="callback">Callback to handle the list of teams.</param>
        public void FetchAllTeams(Action<List<string>> callback)
        {
            _workAsync(new WorkAsyncInfo
            {
                Message = "Fetching teams...",
                Work = (_, args) =>
                {
                    var query = new QueryExpression("team")
                    {
                        ColumnSet = new ColumnSet("name", "teamid")
                    };
                    args.Result = _service.RetrieveMultiple(query).Entities.ToList();
                },
                PostWorkCallBack = args =>
                {
                    if (args.Error != null)
                    {
                        _logger.Log(LogLevel.Error, args.Error.ToString());
                        return;
                    }

                    var teams = (args.Result as List<Entity>).ConvertAll(t => t.GetAttributeValue<string>("name"));
                    _logger.Log(LogLevel.Information, $"Found {teams?.Count ?? 0} teams");
                    callback?.Invoke(teams);
                }
            });
        }

        /// <summary>
        /// Adds a user to a specified team.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <param name="teamId">The ID of the team to add the user to.</param>
        public void AddUserToTeam(Guid userId, Guid teamId)
        {
            var request = new AddMembersTeamRequest
            {
                TeamId = teamId,
                MemberIds = new[] { userId }
            };

            _service.Execute(request);
            _logger.Log(LogLevel.Information, $"Added user {userId} to team {teamId}");
        }

        /// <summary>
        /// Removes a user from a specified team.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        /// <param name="teamId">The ID of the team to remove the user from.</param>
        public void RemoveUserFromTeam(Guid userId, Guid teamId)
        {
            var request = new RemoveMembersTeamRequest
            {
                TeamId = teamId,
                MemberIds = new[] { userId }
            };

            _service.Execute(request);
            _logger.Log(LogLevel.Information, $"Removed user {userId} from team {teamId}");
        }

        /// <summary>
        /// Retrieves team membership (user IDs) for a given team.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <returns>List of user GUIDs.</returns>
        public List<Guid> GetTeamMembers(Guid teamId)
        {
            var query = new QueryExpression("teammembership")
            {
                ColumnSet = new ColumnSet("systemuserid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("teamid", ConditionOperator.Equal, teamId)
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Select(e => e.GetAttributeValue<Guid>("systemuserid")).ToList();
        }

        /// <summary>
        /// Executes bulk add/remove requests for team memberships with progress reporting.
        /// </summary>
        /// <param name="teamId">The team to update.</param>
        /// <param name="userIdsToAdd">Users to add to the team.</param>
        /// <param name="userIdsToRemove">Users to remove from the team.</param>
        public void UpdateTeamMembership(Guid teamId, List<Guid> userIdsToAdd, List<Guid> userIdsToRemove)
        {
            _workAsync(new WorkAsyncInfo
            {
                Message = "Updating team memberships...",
                Work = (worker, args) =>
                {
                    worker.WorkerReportsProgress = true;

                    if (userIdsToRemove?.Any() == true)
                    {
                        var removeRequest = new RemoveMembersTeamRequest
                        {
                            TeamId = teamId,
                            MemberIds = userIdsToRemove.ToArray()
                        };
                        _service.Execute(removeRequest);
                        worker.ReportProgress(50, CreateProgressData("Removed users", $"{userIdsToRemove.Count} removed", null));
                    }

                    if (userIdsToAdd?.Any() == true)
                    {
                        var addRequest = new AddMembersTeamRequest
                        {
                            TeamId = teamId,
                            MemberIds = userIdsToAdd.ToArray()
                        };
                        _service.Execute(addRequest);
                        worker.ReportProgress(100, CreateProgressData("Added users", $"{userIdsToAdd.Count} added", null));
                    }
                },
                ProgressChanged = HandleProgressChanged,
                PostWorkCallBack = HandlePostWorkCallback
            });
        }

        /// <summary>
        /// Creates a tuple with progress data for use in reporting.
        /// </summary>
        private Tuple<string, string, string> CreateProgressData(string message, string status, string error)
        {
            return new Tuple<string, string, string>(message, status, error);
        }

        /// <summary>
        /// Handles progress updates during team membership changes.
        /// </summary>
        private void HandleProgressChanged(ProgressChangedEventArgs args)
        {
            var data = (Tuple<string, string, string>)args.UserState;
            _setWorkingMessage?.Invoke(data.Item1);
            _logger.Log(LogLevel.Information, $"Team update: {data.Item2}, {data.Item1}");
            if (data.Item3 != null)
            {
                _logger.Log(LogLevel.Error, data.Item3);
            }
        }

        /// <summary>
        /// Called after all team update work completes.
        /// </summary>
        private void HandlePostWorkCallback(RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                _logger.Log(LogLevel.Error, args.Error.ToString());
            }
            else if (args.Result is EntityCollection result)
            {
                _logger.Log(LogLevel.Information, $"Operation complete. Affected entities: {result.Entities.Count}");
            }
        }
    }
}