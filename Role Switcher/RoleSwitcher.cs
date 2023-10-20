using XrmToolBox.Extensibility.Interfaces;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Messages;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using McTools.Xrm.Connection;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Data;
using System;
using System.Text;

namespace Role_Switcher
{
    public partial class RoleSwitcher : PluginControlBase, IMessageBusHost, IGitHubPlugin, IAboutPlugin
    {
        private readonly static Playlist EMPTY_PLAYLIST = new Playlist { Id = Guid.Empty, Name = string.Empty, Roles = new List<string>(0) };

        private Settings Settings;
        private RSLogManager Logger;


        private BindingList<Playlist> Playlists = new BindingList<Playlist>() { EMPTY_PLAYLIST };
        private List<string> AllRoles = new List<string>();
        private BindingList<string> RolesToApply = new BindingList<string>();
        private BindingList<string> AssignedRoles = new BindingList<string>();
        private BindingList<string> UnassignedRoles = new BindingList<string>();

        private bool LogsOpen = false;


        private List<Guid> UsersToEdit = new List<Guid> ();

        public string RepositoryName => "Dynamics-Bulk-Role-Updater";

        public string UserName => "JoePittsy";

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleSwitcher"/> class.
        /// </summary>
        public RoleSwitcher()
        {
            InitializeComponent();
            InitializeLogger();
            SetupLogBox();
            SetupDataSourceBindings();
        }

        /// <summary>
        /// Initializes the logger and manages log box scrolling on new log messages.
        /// </summary>
        private void InitializeLogger()
        {
            Logger = new RSLogManager();
            logBox.DataSource = Logger.Messages;
            Logger.Messages.ListChanged += (s, e) =>
            {
                if (e.ListChangedType == ListChangedType.ItemAdded && logBox.IsHandleCreated)
                {
                    logBox.Invoke((MethodInvoker)delegate
                    {
                        logBox.TopIndex = logBox.Items.Count - 1;
                    });
                }
            };
        }

        /// <summary>
        /// Sets up drawing options and events for the log box.
        /// </summary>
        private void SetupLogBox()
        {
            logBox.DrawMode = DrawMode.OwnerDrawFixed;
            logBox.DrawItem += RSLogManager.DrawMessage;
        }

        /// <summary>
        /// Sets up data source bindings for UI elements.
        /// </summary>
        private void SetupDataSourceBindings()
        {
            playlistBindingSource.DataSource = Playlists;
            assignedRolesList.DataSource = AssignedRoles;
            unassingedRolesList.DataSource = UnassignedRoles;
            playlistRoles.DataSource = RolesToApply;
        }



        /// <summary>
        /// Handles the Load event of MyPluginControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }



        /// <summary>
        /// Loads the settings for the plugin, initializing them if necessary.
        /// </summary>
        private void LoadSettings()
        {
            InitializePlaylists();

            if (ConnectionDetail == null)
            {
                LogInfo("Waiting for a connection before loading settings");
                return;
            }

            LoadOrCreateSettings();

            if (Settings != null)
            {
                ApplySettings();
            }
        }

        /// <summary>
        /// Clears existing playlists and adds an empty one.
        /// </summary>
        private void InitializePlaylists()
        {
            Playlists.Clear();
            Playlists.Add(EMPTY_PLAYLIST);
        }

        /// <summary>
        /// Attempts to load settings, creating new ones and logging a warning if loading fails.
        /// </summary>
        private void LoadOrCreateSettings()
        {
            // Try to load settings, and create new ones if they don't exist.
            bool settingsLoaded = SettingsManager.Instance.TryLoad(
                GetType(), out Settings, ConnectionDetail.ConnectionId.ToString()
            );

            // Log appropriate message based on whether settings were loaded.
            if (settingsLoaded)
            {
                Logger.Log(LogLevel.Information, "Settings found and loaded");
            }
            else
            {
                Settings = new Settings();
                Settings.Playlists = new List<Playlist> { };
                Logger.Log(LogLevel.Warning, "Settings not found => a new settings file has been created!");
            }
        }

        /// <summary>
        /// Applies settings to the plugin.
        /// </summary>
        private void ApplySettings()
        {
            // Update logs and UI panels based on settings.
            LogsOpen = Settings.LogsOpen;
            splitContainer.Panel2Collapsed = Settings.LogsOpen;

            // Load playlists from settings and log information about them.
            foreach (var playlist in Settings.Playlists)
            {
                Playlists.Add(playlist);
                Logger.Log(LogLevel.Information, $"Loaded Playlist {playlist.Name}:{playlist.Id}");
            }
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAllUsers);
        }


        /// <summary>
        /// Converts a list of dictionaries into a DataTable.
        /// </summary>
        /// <param name="data">A list of dictionaries containing the data to be converted.</param>
        /// <returns>A DataTable containing the data from the input list.</returns>
        private DataTable ConvertToDataTable(List<Dictionary<string, string>> data)
        {
            DataTable dataTable = new DataTable();

            // Return an empty DataTable if no data is provided.
            if (data.Count == 0)
            {
                return dataTable;
            }

            // Extract and sanitize column names, then add them to the DataTable.
            AddColumnsToDataTable(data, dataTable);

            // Add rows to the DataTable.
            AddRowsToDataTable(data, dataTable);

            return dataTable;
        }

        /// <summary>
        /// Extracts and sanitizes column names, and adds them as columns to the DataTable.
        /// </summary>
        /// <param name="data">A list of dictionaries containing the data to extract columns from.</param>
        /// <param name="dataTable">The DataTable to add columns to.</param>
        private void AddColumnsToDataTable(List<Dictionary<string, string>> data, DataTable dataTable)
        {
            // Get a distinct list of all keys from the provided data, to be used as column names.
            var allKeys = data.SelectMany(dict => dict.Keys).Distinct().ToList();

            // Add columns to the DataTable.
            foreach (var key in allKeys)
            {
                string sanitizedColumnName = SanitizeColumnName(key);
                Logger.Log(LogLevel.Information, $"Adding {sanitizedColumnName} to the table");

                // Add a new column of type string, with the sanitized name.
                dataTable.Columns.Add(sanitizedColumnName, typeof(string));

                // Set the caption of the column, which can be used for display purposes.
                dataTable.Columns[sanitizedColumnName].Caption = key;
            }
        }

        /// <summary>
        /// Adds rows to the DataTable based on the provided data.
        /// </summary>
        /// <param name="data">A list of dictionaries containing the data to be converted.</param>
        /// <param name="dataTable">The DataTable to add rows to.</param>
        private void AddRowsToDataTable(List<Dictionary<string, string>> data, DataTable dataTable)
        {
            // Add data to the DataTable.
            foreach (var dict in data)
            {
                DataRow row = dataTable.NewRow();

                // Populate the row with data from the dictionary.
                foreach (var key in dict.Keys)
                {
                    string sanitizedColumnName = SanitizeColumnName(key);
                    row[sanitizedColumnName] = dict[key]?.ToString() ?? string.Empty;
                }

                dataTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Sanitizes a column name by replacing problematic characters.
        /// </summary>
        /// <param name="columnName">The original column name to sanitize.</param>
        /// <returns>The sanitized column name.</returns>
        private string SanitizeColumnName(string columnName)
        {
            return columnName.Replace(".", "_");
        }

        /// <summary>
        /// Initiates an asynchronous operation to fetch user data utilizing a specified fetch method.
        /// </summary>
        /// <param name="fetchMethod">A delegate that defines the method used to fetch the user data.</param>
        private void FetchUsers(Func<IOrganizationService, object> fetchMethod)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Fetching users...",
                Work = (worker, args) => args.Result = fetchMethod(Service),
                PostWorkCallBack = ProcessFetchedUsers
            });
        }

        /// <summary>
        /// Processes the fetched user data, logging any errors and updating the UI if the fetch is successful.
        /// </summary>
        /// <param name="args">Contains the results of the asynchronous operation.</param>
        private void ProcessFetchedUsers(RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                Logger.Log(LogLevel.Error, args.Error.ToString());
                return;
            }

            var result = args.Result as EntityCollection;
            if (result != null)
            {
                UpdateUserGrid(result);
                Logger.Log(LogLevel.Information, $"Found {result.Entities.Count} users");
            }
        }

        /// <summary>
        /// Updates the UI grid to display the fetched user data.
        /// </summary>
        /// <param name="result">Contains the user data to display in the grid.</param>
        private void UpdateUserGrid(EntityCollection result)
        {
            var data = result.Entities.Select(e => e.FormattedValues.ToDictionary(a => a.Key, a => a.Value)).ToList();
            userGrid.DataSource = ConvertToDataTable(data);
            userGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Fetches all users using a QueryExpression and updates the UI grid with the fetched data.
        /// </summary>
        private void GetAllUsers()
        {
            // Utilizing FetchUsers and providing a method for retrieving all users using QueryExpression.
            FetchUsers(service => service.RetrieveMultiple(new QueryExpression("systemuser") { ColumnSet = new ColumnSet("fullname") }));
        }

        /// <summary>
        /// Fetches users based on the provided FetchXML query and updates the UI grid with the fetched data.
        /// </summary>
        /// <param name="fetchXML">A FetchXML query used to retrieve user data.</param>
        private void GetUsers(string fetchXML)
        {
            // Utilizing FetchUsers and providing a method for retrieving users using FetchExpression.
            FetchUsers(service => service.RetrieveMultiple(new FetchExpression(fetchXML)));
        }

        /// <summary>
        /// Fetches all roles from the default Business Unit and logs the results.
        /// </summary>
        private void GetRoles()
        {
            FetchEntities("Fetching all roles...",
            service =>
            {
                var defaultBuQuery = new QueryExpression("businessunit")
                {
                    ColumnSet = new ColumnSet("businessunitid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                    new ConditionExpression("parentbusinessunitid", ConditionOperator.Null)
                        }
                    }
                };
                var defaultBu = service.RetrieveMultiple(defaultBuQuery).Entities.First().Id;

                var roleQuery = new QueryExpression("role")
                {
                    ColumnSet = new ColumnSet("name"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                    new ConditionExpression("businessunitid", ConditionOperator.Equal, defaultBu)
                        }
                    }
                };

                return service.RetrieveMultiple(roleQuery);
            },
            ProcessFetchedRoles);
        }

        /// <summary>
        /// Processes and logs the fetched roles, updating a collection with the results.
        /// </summary>
        /// <param name="args">Contains the results of the asynchronous operation.</param>
        private void ProcessFetchedRoles(RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                Logger.Log(LogLevel.Error, args.Error.ToString());
                return;
            }

            var result = args.Result as EntityCollection;
            if (result != null)
            {
                AllRoles = result.Entities.Select(e => e.GetAttributeValue<string>("name")).ToList();
                Logger.Log(LogLevel.Information, $"Found {result.Entities.Count} roles");
            }
        }

        /// <summary>
        /// Initiates an asynchronous operation to fetch CRM entity data utilizing a specified fetch method.
        /// </summary>
        /// <param name="fetchMessage">Message to be displayed during data fetch.</param>
        /// <param name="fetchMethod">A delegate that defines the method used to fetch the entity data.</param>
        /// <param name="callback">Callback function to process and handle fetched data.</param>
        private void FetchEntities(string fetchMessage, Func<IOrganizationService, object> fetchMethod, Action<RunWorkerCompletedEventArgs> callback)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = fetchMessage,
                Work = (worker, args) => args.Result = fetchMethod(Service),
                PostWorkCallBack = callback
            });
        }


        /// <summary>
        /// Saves the current settings of the application. 
        /// </summary>
        /// <remarks>
        /// This method will save settings such as the state of the logs and playlists,
        /// excluding the EMPTY_PLAYLIST, into a persistent storage for future sessions.
        /// Settings are saved using a unique identifier associated with the current connection.
        /// No settings will be saved if there is no active connection or if mySettings is null.
        /// </remarks>
        private void SaveSettings()
        {
            if (ConnectionDetail == null || Settings == null)
            {
                Logger.Log(LogLevel.Warning, "Failed to save settings: No active connection or settings are null.");
                return;
            }

            Logger.Log(LogLevel.Information, "Saving Settings");

            UpdateSettings();
            SettingsManager.Instance.Save(GetType(), Settings, ConnectionDetail.ConnectionId.ToString());
        }

        /// <summary>
        /// Updates the settings object with the current state of the application.
        /// </summary>
        private void UpdateSettings()
        {
            Settings.LogsOpen = LogsOpen;
            // Store current playlists, excluding the empty one.
            Settings.Playlists = Playlists
                                    .ToList()
                                    .Except(new List<Playlist> { EMPTY_PLAYLIST })
                                    .ToList();
        }



        /// <summary>
        /// Handles the logic to be executed when the plugin is closing.
        /// </summary>
        /// <param name="info">Information related to the plugin closing event.</param>
        /// <remarks>
        /// This method saves the current settings before calling the base implementation 
        /// of ClosingPlugin to ensure any additional closing logic in the base class is executed.
        /// </remarks>
        public override void ClosingPlugin(PluginCloseInfo info)
        {
            SaveSettings();
            base.ClosingPlugin(info);
        }

        /// <summary>
        /// Updates the connection and refreshes the related configurations and settings.
        /// </summary>
        /// <param name="newService">The new organization service.</param>
        /// <param name="detail">The connection detail.</param>
        /// <param name="actionName">The name of the action that triggered the update.</param>
        /// <param name="parameter">Additional parameter data for the connection update.</param>
        /// <remarks>
        /// This is triggered when there is a change in connection in the XrmToolBox.
        /// It clears role caches, saves & loads settings, logs the new connection, and retrieves roles afresh.
        /// </remarks>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            applyButton.Enabled = true;
            ClearRoleCollections();

            SaveSettings();
            base.UpdateConnection(newService, detail, actionName, parameter);
            LoadSettings();

            LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            GetRoles();
        }

        /// <summary>
        /// Clears collections related to roles to prevent data inconsistency upon connection change.
        /// </summary>
        private void ClearRoleCollections()
        {
            AllRoles.Clear();
            RolesToApply.Clear();
            AssignedRoles.Clear();
            UnassignedRoles.Clear();
        }

        /// <summary>
        /// Handles incoming messages from the MessageBus.
        /// </summary>
        /// <param name="message">The incoming message to be processed.</param>
        /// <remarks>
        /// This method specifically listens for messages from "FetchXML Builder" and,
        /// upon receiving a valid FetchXML string, initiates a user retrieval based on it.
        /// </remarks>
        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            if (message.SourcePlugin == "FetchXML Builder" &&
                message.TargetArgument is string fetchxml &&
                !string.IsNullOrWhiteSpace(fetchxml))
            {
                GetUsers(fetchxml);
            }
        }


        /// <summary>
        /// Handles the click event of the fetchXMLButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Initiates an outgoing message to "FetchXML Builder" with a predefined FetchXML string for the systemuser entity.
        /// </remarks>
        private void fetchXMLButton_Click(object sender, EventArgs e)
        {
            OnOutgoingMessage(this, new MessageBusEventArgs("FetchXML Builder")
            {
                TargetArgument = "<fetch><entity name=\"systemuser\"/></fetch>"
            });
        }


        /// <summary>
        /// Handles the click event of the toggleLogsButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Toggles the visibility state of the logs and updates the UI and settings accordingly.
        /// </remarks>
        private void toggleLogsButton_Click(object sender, EventArgs e)
        {
            LogsOpen = !LogsOpen;
            if (Settings != null) Settings.LogsOpen = LogsOpen;
            splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
        }

        #region Playlist Specific Methods

        /// <summary>
        /// Generates a unique Guid that is not already used as an ID in the Playlists.
        /// </summary>
        /// <returns>A unique Guid not present in the Playlists.</returns>
        /// <remarks>
        /// The method recursively calls itself if a generated Guid is already present in the Playlists,
        /// ensuring the returned Guid is unique among them.
        /// </remarks>
        private Guid GenerateGuid()
        {
            Guid guid = Guid.NewGuid();
            if (Playlists.Any(p => p.Id == guid)) return GenerateGuid();
            return guid;
        }


        /// <summary>
        /// Handles the click event of the newPlaylist button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Creates a new playlist with a unique identifier, updates the UI, adds it to the Playlists collection, 
        /// and saves the settings. The newly created playlist is then selected in the editPlaylistComboBox.
        /// </remarks>
        private void newPlaylist_Click(object sender, EventArgs e)
        {
            var newPlaylist = new Playlist()
            {
                Name = "New Playlist",
                Id = GenerateGuid(),
                Roles = new List<string>()
            };
            editPlaylistName.Text = newPlaylist.Name;
            Playlists.Add(newPlaylist);
            SaveSettings();
            editPlaylistComboBox.SelectedItem = newPlaylist;
        }

        /// <summary>
        /// Handles the click event for the deleteButton to delete a selected playlist.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// When a playlist is selected and the delete button is clicked, a confirmation message box appears.
        /// If the user confirms deletion, the selected playlist is removed from the Playlists collection, 
        /// settings are saved, and the selected item in the editPlaylistComboBox is set to EMPTY_PLAYLIST.
        /// Deletion of EMPTY_PLAYLIST is prevented.
        /// </remarks>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            Playlist selectedPlaylist = (Playlist)editPlaylistComboBox.SelectedItem;
            if (selectedPlaylist == null || selectedPlaylist == EMPTY_PLAYLIST) return;

            var result = MessageBox.Show($"Are you sure you want to delete {selectedPlaylist.Name}? \n This action is irreversible",
                                         "Are you Sure?",
                                         MessageBoxButtons.OKCancel,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                Playlists.Remove(selectedPlaylist);
                editPlaylistComboBox.SelectedItem = EMPTY_PLAYLIST;
                SaveSettings();
            }
        }


        /// <summary>
        /// Saves the modified playlist data.
        /// </summary>
        /// <remarks>
        /// The method performs the following actions:
        /// - Retrieves the selected playlist from the combo box and updates its properties.
        /// - Refreshes the data bindings to reflect the changes in the UI.
        /// - Saves the current settings.
        /// </remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void savePlaylist_Click(object sender, EventArgs e)
        {
            // Retrieve the selected playlist and update its properties.
            Playlist selectedPlaylist = (Playlist)editPlaylistComboBox.SelectedItem;
            selectedPlaylist.Name = editPlaylistName.Text;
            selectedPlaylist.Roles = AssignedRoles.ToList();

            // Refresh the data bindings.
            playlistBindingSource.ResetBindings(false);

            // Persist the updated settings.
            SaveSettings();
        }


        /// <summary>
        /// Handles the event when a playlist selection is changed.
        /// Updates role assignments and UI components accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void editPlaylistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Playlist selectedPlaylist = (Playlist)editPlaylistComboBox.SelectedItem;

            // Return early if no playlist is selected.
            if (selectedPlaylist == null) return;

            AssignedRoles.Clear();
            UnassignedRoles.Clear();

            // Handle UI updates and role assignments based on the selected playlist.
            if (selectedPlaylist == EMPTY_PLAYLIST)
            {
                editPlaylistName.Text = "";
            }
            else
            {
                editPlaylistName.Text = selectedPlaylist.Name;

                foreach (var role in selectedPlaylist.Roles)
                {
                    AssignedRoles.Add(role);
                }

                foreach (var role in AllRoles.Except(selectedPlaylist.Roles))
                {
                    UnassignedRoles.Add(role);
                }
            }
        }


        /// <summary>
        /// Handles the event when the "Assign" button is clicked.
        /// Moves selected roles from the "unassigned" list to the "assigned" list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void assignButton_Click(object sender, EventArgs e)
        {
            var rolesToChange = new string[unassingedRolesList.SelectedItems.Count];
            unassingedRolesList.SelectedItems.CopyTo(rolesToChange, 0);

            foreach (string role in rolesToChange)
            {
                AssignedRoles.Add(role);
                UnassignedRoles.Remove(role);
            }
        }


        /// <summary>
        /// Handles the event when the "Unassign" button is clicked.
        /// Moves selected roles from the "assigned" list to the "unassigned" list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void unassignButton_Click(object sender, EventArgs e)
        {
            var rolesToChange = new string[assignedRolesList.SelectedItems.Count];
            assignedRolesList.SelectedItems.CopyTo(rolesToChange, 0);

            foreach (string role in rolesToChange)
            {
                UnassignedRoles.Add(role);
                AssignedRoles.Remove(role);
            }
        }


        /// <summary>
        /// Event handler for the change of selected item in the playlistComboBox.
        /// Updates the rolesToApply list based on the roles within the selected playlist.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments (not used in this method).</param>
        private void playlistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Playlist selectedPlaylist = (Playlist)playlistComboBox.SelectedItem;
            if (selectedPlaylist == null) return;
            RolesToApply.Clear();
            if (selectedPlaylist != EMPTY_PLAYLIST)
            {
                foreach (string role in selectedPlaylist.Roles) { RolesToApply.Add(role); }
            }
        }

        #endregion


        /// <summary>
        /// Retrieves roles for business units and updates user roles accordingly.
        /// </summary>
        /// <param name="replaceRoles">Indicates whether to replace existing roles.</param>
        private void GetRolesForBUs(bool replaceRoles)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Fetching roles...",
                Work = (worker, args) =>
                {
                    args.Result = RolesToApply.Count == 0 ? new EntityCollection() : RetrieveRoles();
                },
                PostWorkCallBack = (args) =>
                {
                    HandleRoleRetrievalResponse(args, replaceRoles);
                }
            });
        }

        /// <summary>
        /// Retrieve roles based on the roles specified in rolesToApply.
        /// </summary>
        /// <returns>A result from the role retrieval query.</returns>
        private EntityCollection RetrieveRoles()
        {
            var query = new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("roleid", "businessunitid")
            };
            var roleCriteria = new FilterExpression(LogicalOperator.Or);
            query.Criteria.AddFilter(roleCriteria);

            foreach (var role in RolesToApply)
            {
                roleCriteria.AddCondition("name", ConditionOperator.Equal, role);
            }

            return Service.RetrieveMultiple(query);
        }

        /// <summary>
        /// Handles the response from the role retrieval operation and updates user roles accordingly.
        /// </summary>
        /// <param name="args">The arguments from the PostWorkCallBack.</param>
        /// <param name="replaceRoles">Indicates whether to replace existing roles.</param>
        private void HandleRoleRetrievalResponse(RunWorkerCompletedEventArgs args, bool replaceRoles)
        {
            if (args.Error != null)
            {
                Logger.Log(LogLevel.Error, args.Error.ToString());
            }

            var result = args.Result as EntityCollection;

            var buRoles = result?.Entities.Select(e => new Tuple<Guid, string, Guid>(
                e.GetAttributeValue<EntityReference>("businessunitid").Id,
                e.GetAttributeValue<string>("name"),
                e.Id))
            .ToList() ?? new List<Tuple<Guid, string, Guid>>();

            Logger.Log(LogLevel.Information, $"Found {result?.Entities.Count ?? 0} roles");

            UpdateUsersRoles(buRoles, replaceRoles);
        }



        /// <summary>
        /// Updates roles for users asynchronously, providing progress updates.
        /// </summary>
        /// <param name="buRoles">A list of business unit roles.</param>
        /// <param name="replaceRoles">Whether to replace existing roles.</param>
        private void UpdateUsersRoles(List<Tuple<Guid, string, Guid>> buRoles, bool replaceRoles)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Assigning Roles...",
                Work = (worker, args) =>
                {
                    worker.WorkerReportsProgress = true;
                    AssignRolesToUsers(worker, buRoles, replaceRoles);
                },
                ProgressChanged = args =>
                {
                    HandleProgressChanged(args);
                },
                PostWorkCallBack = args =>
                {
                    HandlePostWorkCallback(args);
                }
            });
        }


        /// <summary>
        /// Handles progress changed events during the role assignment process.
        /// </summary>
        /// <param name="args">Arguments from the progress changed event.</param>
        private void HandleProgressChanged(ProgressChangedEventArgs args)
        {
            var data = (Tuple<string, string, string>)args.UserState;
            SetWorkingMessage(data.Item1);
            Logger.Log(LogLevel.Information, $"Applied roles to {data.Item2}, {data.Item1}");
            if (data.Item3 != null) Logger.Log(LogLevel.Error, data.Item3);
        }

        /// <summary>
        /// Handles post work callback, performing any cleanup or final logging.
        /// </summary>
        /// <param name="args">Arguments from the post work callback event.</param>
        private void HandlePostWorkCallback(RunWorkerCompletedEventArgs args)
        {
            UsersToEdit.Clear();
            if (args.Error != null)
            {
                Logger.Log(LogLevel.Error, args.Error.ToString());
            }
            if (args.Result is EntityCollection result)
            {
                Logger.Log(LogLevel.Information, $"Found {result.Entities.Count} roles");
            }
        }


        /// <summary>
        /// Creates a tuple representing progress data.
        /// </summary>
        /// <param name="message">A progress message.</param>
        /// <param name="status">A status message indicating the amount of work done.</param>
        /// <param name="error">An error message, if applicable.</param>
        /// <returns>A tuple containing the progress data.</returns>
        private Tuple<string, string, string> CreateProgressData(string message, string status, string error)
        {
            return new Tuple<string, string, string>(message, status, error);
        }
        /// <summary>
        /// Assigns roles to users with progress tracking.
        /// </summary>
        /// <param name="worker">Background worker to report progress.</param>
        /// <param name="buRoles">A list of business unit roles.</param>
        /// <param name="replaceRoles">Whether to replace existing roles.</param>
        private void AssignRolesToUsers(BackgroundWorker worker, List<Tuple<Guid, string, Guid>> buRoles, bool replaceRoles)
        {
            worker.ReportProgress(0, CreateProgressData("Assigning Roles...", "0% Done...", null));

            // Perform user query and group results by user
            var grouped = QueryUsersAndGroup(replaceRoles);

            // Now, for each user we need to update roles
            var usersDone = 0;
            foreach (var group in grouped)
            {
                usersDone++;
                // Fetching necessary user details
                var userId = group.FirstOrDefault().Id;
                var usersBuId = group.FirstOrDefault().GetAttributeValue<EntityReference>("businessunitid").Id;
                var rolesToAdd = buRoles.Where(r => r.Item1 == usersBuId).ToList();

                // Create and execute requests to update roles
                ExecuteRoleUpdates(userId, rolesToAdd, group, replaceRoles);

                // Report progress
                var progress = (int)Math.Ceiling(((float)usersDone / (float)grouped.Count()) * 100);
                worker.ReportProgress(progress, new Tuple<string, string, string>($"{progress}% Done...", group.Key, null));
            }
        }

        /// <summary>
        /// Queries users and groups the results.
        /// </summary>
        /// <param name="replaceRoles">Whether to replace existing roles.</param>
        /// <returns>Grouped query results.</returns>
        private IEnumerable<IGrouping<string, Entity>> QueryUsersAndGroup(bool replaceRoles)
        {
            var query = new QueryExpression("systemuser");
            query.ColumnSet.AddColumns("fullname", "systemuserid", "businessunitid");
            var query_Or = new FilterExpression(LogicalOperator.Or);
            query.Criteria.AddFilter(query_Or);
            foreach (var user in UsersToEdit) query_Or.AddCondition("systemuserid", ConditionOperator.Equal, user);

            if (replaceRoles)
            {
                // Additional query logic for replacing roles
                var rolemembership = query.AddLink("systemuserroles", "systemuserid", "systemuserid", JoinOperator.LeftOuter);
                rolemembership.EntityAlias = "rolemembership";

                var role = rolemembership.AddLink("role", "roleid", "roleid", JoinOperator.LeftOuter);
                role.EntityAlias = "role";
                role.Columns.AddColumn("roleid");
            }

            var results = Service.RetrieveMultiple(query);
            return results.Entities.GroupBy(e => $"{e.GetAttributeValue<string>("fullname")}:{e.Id}").ToList();
        }

        /// <summary>
        /// Executes requests to update roles.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="rolesToAdd">List of roles to add.</param>
        /// <param name="group">Group of user entities.</param>
        /// <param name="replaceRoles">Whether to replace existing roles.</param>
        private void ExecuteRoleUpdates(Guid userId, List<Tuple<Guid, string, Guid>> rolesToAdd, IGrouping<string, Entity> group, bool replaceRoles)
        {
            var associateRequest = CreateAssociateRequests(userId, rolesToAdd);
            var disassociateRequest = CreateDisassociateRequests(userId, group, replaceRoles);

            var multiDisassociateRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = disassociateRequest
            };

            var mutliAssociateRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = associateRequest
            };

            var dResult = (ExecuteMultipleResponse)Service.Execute(multiDisassociateRequest);
            var aResult = (ExecuteMultipleResponse)Service.Execute(mutliAssociateRequest);

            HandleRoleUpdateResults(dResult);
            HandleRoleUpdateResults(aResult);

        }

        /// <summary>
        /// Creates an associate request to assign roles to a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="rolesToAdd">Roles to be added to the user.</param>
        /// <returns>An associate request.</returns>
        private OrganizationRequestCollection CreateAssociateRequests(Guid userId, List<Tuple<Guid, string, Guid>> rolesToAdd)
        {
            var requests = new OrganizationRequestCollection();
            foreach (var role in rolesToAdd)
            {
                var associateRequest = new AssociateRequest
                {
                    Target = new EntityReference("systemuser", userId),
                    RelatedEntities = new EntityReferenceCollection(),
                    Relationship = new Relationship("systemuserroles_association")
                };

                associateRequest.RelatedEntities.Add(new EntityReference("systemrole", role.Item3));
                requests.Add(associateRequest);
            }

            return requests;
        }


        /// <summary>
        /// Creates a disassociate request to remove roles from a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="group">Group of user entities.</param>
        /// <param name="replaceRoles">Whether to replace existing roles.</param>
        /// <returns>A disassociate request.</returns>
        private OrganizationRequestCollection CreateDisassociateRequests(Guid userId, IGrouping<string, Entity> group, bool replaceRoles)
        {
            var requests = new OrganizationRequestCollection ();

            if (replaceRoles)
            {
                foreach (var role in group)
                {
                    var disassociateRequest = new DisassociateRequest
                    {
                        Target = new EntityReference("systemuser", userId),
                        RelatedEntities = new EntityReferenceCollection(),
                        Relationship = new Relationship("systemuserroles_association")
                    };
                    if (role.TryGetAttributeValue("role.roleid", out AliasedValue roleId))
                    {
                        disassociateRequest.RelatedEntities.Add(new EntityReference("systemrole", (Guid)roleId.Value));
                        requests.Add(disassociateRequest);
                    }
                }
            }

            return requests;
        }


        /// <summary>
        /// Handles the results of role update requests.
        /// </summary>
        /// <param name="result">The result from executing multiple requests.</param>
        private void HandleRoleUpdateResults(ExecuteMultipleResponse result)
        {
            foreach (var responseItem in result.Responses)
            {
                // Check for errors/faults.
                if (responseItem.Fault != null)
                {
                    Logger.Log(LogLevel.Error, $"Error updating roles: {responseItem.Fault.Message}");
                }
            }
        }


        /// <summary>
        /// Handles the Click event of the applyButton control. It applies selected roles from the playlist to selected users.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void applyButton_Click(object sender, EventArgs e)
        {
            var playlist = (Playlist)playlistComboBox.SelectedItem;
            if (playlist == EMPTY_PLAYLIST) return;
            var users = userGrid.SelectedRows;
            bool replaceRoles = removeRolesCheck.Checked;

            if (ConfirmRoleApplication(users.Count, playlist.Name, replaceRoles) == DialogResult.Cancel)
                return;

            UsersToEdit.Clear();
            ExtractUserIds(users);
            GetRolesForBUs(replaceRoles);
        }

        /// <summary>
        /// Extracts and logs user IDs from the grid selection, adding them to the usersToEdit list.
        /// </summary>
        /// <param name="users">The selected users in the grid.</param>
        private void ExtractUserIds(DataGridViewSelectedRowCollection users)
        {
            int idColumnIndex = userGrid.Columns.IndexOf(userGrid.Columns["systemuserid"]);

            foreach (DataGridViewRow user in users)
            {
                var id = (string)user.Cells[idColumnIndex].Value;
                if (id == null) continue;
                Logger.Log(LogLevel.Information, $"User ID: {id}");
                UsersToEdit.Add(new Guid(id));
            }
        }

        /// <summary>
        /// Displays a confirmation dialog for role application.
        /// </summary>
        /// <param name="userCount">The number of users to apply roles to.</param>
        /// <param name="playlistName">The name of the playlist.</param>
        /// <param name="replaceRoles">If set to <c>true</c>, indicates current roles of the users will be replaced.</param>
        /// <returns>The dialog result of the confirmation dialog.</returns>
        private DialogResult ConfirmRoleApplication(int userCount, string playlistName, bool replaceRoles)
        {
            var message = BuildConfirmationMessage(userCount, playlistName, replaceRoles);
            return MessageBox.Show(message, "Apply Roles?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Builds the confirmation message string for role application.
        /// </summary>
        /// <param name="userCount">The number of users to apply roles to.</param>
        /// <param name="playlistName">The name of the playlist.</param>
        /// <param name="replaceRoles">If set to <c>true</c>, indicates current roles of the users will be replaced.</param>
        /// <returns>The constructed confirmation message.</returns>
        private string BuildConfirmationMessage(int userCount, string playlistName, bool replaceRoles)
        {
            var message = new StringBuilder($"You are about to apply the playlist {playlistName} to {userCount} users");

            if (replaceRoles)
                message.Append(" and remove all their current roles");

            message.Append(".\n Are you sure?");

            return message.ToString();
        }

        public void ShowAboutDialog()
        {
            var about = new AboutDialog();
            about.ShowDialog();
        }
    }
}