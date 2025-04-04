﻿using XrmToolBox.Extensibility.Interfaces;
using System.Collections.Generic;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using McTools.Xrm.Connection;
using Role_Switcher.Services;
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
        private static readonly Playlist EMPTY_PLAYLIST = new Playlist { Id = Guid.Empty, Name = string.Empty, Roles = new List<string>(0), Teams = new List<string>(0) };

        private Settings Settings;
        private RSLogManager Logger;

        private BindingList<Playlist> Playlists = new BindingList<Playlist>() { EMPTY_PLAYLIST };

        private List<string> AllRoles = new List<string>();
        private BindingList<string> RolesToApply = new BindingList<string>();
        private BindingList<string> AssignedRoles = new BindingList<string>();
        private BindingList<string> UnassignedRoles = new BindingList<string>();

        private List<string> AllTeams = new List<string>();
        private BindingList<string> TeamsToApply = new BindingList<string>();
        private BindingList<string> AssignedTeams = new BindingList<string>();
        private BindingList<string> UnassignedTeams = new BindingList<string>();

        private StringBuilder SearchText = new StringBuilder();
        private Timer SearchTimer = new Timer();
        private bool LogsOpen = false;

        private List<Guid> UsersToEdit = new List<Guid>();

        private RoleService _roleService;
        private UserGridBuilder _userGridBuilder = new UserGridBuilder();
        private PlaylistService _playlistService;
        private TeamsService _teamsService;

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
            InitializeUserGrid();
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
            assignedTeamsList.DataSource = AssignedTeams;
            unasssignedTeamsList.DataSource = UnassignedTeams;
            unassingedRolesList.DataSource = UnassignedRoles;
            playlistRoles.DataSource = RolesToApply;
            playlistTeams.DataSource = TeamsToApply;
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
                Settings = new Settings
                {
                    Playlists = new List<Playlist>()
                };
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
        /// Initializes the components related to the DataGridView and its search functionality.
        /// </summary>
        private void InitializeUserGrid()
        {
            userGrid.KeyPress += UserGrid_KeyPress;

            SearchTimer.Interval = 1000;  // 1 second
            SearchTimer.Tick += SearchTimer_Tick;
        }

        /// <summary>
        /// Handles the KeyPress event of the DataGridView to facilitate the search-as-you-type functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data.</param>
        private void UserGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (char.IsLetterOrDigit(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
                {
                    SearchText.Append(e.KeyChar);
                    _userGridBuilder.SearchGrid(userGrid, SearchText.ToString());
                    ResetSearchTimer();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as per your application's guidelines.
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Resets the search timer, which determines the duration before the accumulated search text is cleared.
        /// </summary>
        private void ResetSearchTimer()
        {
            SearchTimer.Stop();
            SearchTimer.Start();
        }

        /// <summary>
        /// Handles the Tick event of the searchTimer, which is used to clear the accumulated search text after a specific interval.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            SearchTimer.Stop();
            SearchText.Clear();
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
                Work = (_, args) => args.Result = fetchMethod(Service),
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

            if (args.Result is EntityCollection result)
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
            userGrid.DataSource = _userGridBuilder.BuildUserTable(result.Entities);
            userGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Fetches all users using a QueryExpression and updates the UI grid with the fetched data.
        /// </summary>
        private void GetAllUsers()
        {
            // Utilizing FetchUsers and providing a method for retrieving all users using QueryExpression.
            FetchUsers(service => service.RetrieveMultiple(new QueryExpression("systemuser") { ColumnSet = new ColumnSet("firstname", "lastname") }));
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
            _roleService.FetchAllRoleNamesFromDefaultBU((allRoles) =>
            {
                AllRoles = allRoles;
                Logger.Log(LogLevel.Information, $"Found {allRoles.Count} roles");
            });
        }

        private void GetTeams()
        {
            _teamsService.FetchAllTeams((allTeams) =>
            {
                AllTeams = allTeams;
                Logger.Log(LogLevel.Information, $"Found {allTeams.Count} teams");
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
            ClearTeamCollections();
            ClearUsers();

            SaveSettings();
            base.UpdateConnection(newService, detail, actionName, parameter);

            _roleService = new RoleService(Service, Logger, WorkAsync, m => SetWorkingMessage(m));
            _teamsService = new TeamsService(Service, Logger, WorkAsync, m => SetWorkingMessage(m));
            _playlistService = new PlaylistService(Playlists, EMPTY_PLAYLIST, SaveSettings);

            LoadSettings();

            LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            GetRoles();
            GetTeams();
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
        /// Clears collections related teams roles to prevent data inconsistency upon connection change.
        /// </summary>
        private void ClearTeamCollections()
        {
            AllTeams.Clear();
            TeamsToApply.Clear();
            AssignedTeams.Clear();
            UnassignedTeams.Clear();
        }

        /// <summary>
        /// Clears the User Datagrid view and UsersToEdit selection.
        /// </summary>
        private void ClearUsers()
        {
            UpdateUserGrid(new EntityCollection());
            UsersToEdit.Clear();
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
            var newPlaylist = _playlistService.CreateNewPlaylist();
            editPlaylistName.Text = newPlaylist.Name;
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
            var selected = (Playlist)editPlaylistComboBox.SelectedItem;
            var result = MessageBox.Show($"Are you sure you want to delete {selected.Name}? \n This action is irreversible",
                             "Are you Sure?",
                             MessageBoxButtons.OKCancel,
                             MessageBoxIcon.Question);

            if (result == DialogResult.OK && _playlistService.TryDeletePlaylist(selected))
            {
                editPlaylistComboBox.SelectedItem = EMPTY_PLAYLIST;
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
            var selected = (Playlist)editPlaylistComboBox.SelectedItem;
            _playlistService.UpdatePlaylist(selected, editPlaylistName.Text, AssignedRoles, AssignedTeams);

            playlistBindingSource.ResetBindings(false);
            RolesToApply.ResetBindings();
            playlistComboBox.SelectedIndex = 0;
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
            AssignedTeams.Clear();
            UnassignedTeams.Clear();

            // Handle UI updates and role assignments based on the selected playlist.
            if (selectedPlaylist == EMPTY_PLAYLIST)
            {
                editPlaylistName.Text = "";
            }
            else
            {
                editPlaylistName.Text = selectedPlaylist.Name;

                foreach (var role in selectedPlaylist.Roles) AssignedRoles.Add(role);
                foreach (var role in AllRoles.Except(selectedPlaylist.Roles)) UnassignedRoles.Add(role);

                foreach (var team in selectedPlaylist.Teams) AssignedTeams.Add(team);
                foreach (var team in AllTeams.Except(selectedPlaylist.Teams)) UnassignedTeams.Add(team);
            }

            assignedTeamsList.SelectedIndex = -1;
            unasssignedTeamsList.SelectedIndex = -1;
            assignedRolesList.SelectedIndex = -1;
            unassingedRolesList.SelectedIndex = -1;
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

            var teamsToChange = new string[unasssignedTeamsList.SelectedItems.Count];
            unasssignedTeamsList.SelectedItems.CopyTo(teamsToChange, 0);

            foreach (string team in teamsToChange)
            {
                AssignedTeams.Add(team);
                UnassignedTeams.Remove(team);
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

            var teamsToChange = new string[assignedTeamsList.SelectedItems.Count];
            assignedTeamsList.SelectedItems.CopyTo(teamsToChange, 0);

            foreach (string team in teamsToChange)
            {
                UnassignedTeams.Add(team);
                AssignedTeams.Remove(team);
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
            TeamsToApply.Clear();
            if (selectedPlaylist != EMPTY_PLAYLIST)
            {
                foreach (string role in selectedPlaylist.Roles) { RolesToApply.Add(role); }
                foreach (string team in selectedPlaylist.Teams) { TeamsToApply.Add(team); }
            }
        }

        #endregion Playlist Specific Methods

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
            bool replaceTeams = removeTeamsCheck.Checked;

            if (ConfirmRoleApplication(users.Count, playlist.Name, replaceRoles, replaceTeams) == DialogResult.Cancel)
                return;

            var userGuids = _userGridBuilder.ExtractUserIds(userGrid);
            _roleService.ApplyRolesToUsers(userGuids, playlist.Roles, replaceRoles);
            _teamsService.ApplyTeamsToUsers(userGuids, playlist.Teams, replaceTeams);
        }

        /// <summary>
        /// Displays a confirmation dialog for role application.
        /// </summary>
        /// <param name="userCount">The number of users to apply roles to.</param>
        /// <param name="playlistName">The name of the playlist.</param>
        /// <param name="replaceRoles">If set to <c>true</c>, indicates current roles of the users will be replaced.</param>
        /// <returns>The dialog result of the confirmation dialog.</returns>
        private DialogResult ConfirmRoleApplication(int userCount, string playlistName, bool replaceRoles, bool replaceTeams)
        {
            var message = BuildConfirmationMessage(userCount, playlistName, replaceRoles, replaceTeams);
            return MessageBox.Show(message, "Apply Roles?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Builds the confirmation message string for role application.
        /// </summary>
        /// <param name="userCount">The number of users to apply roles to.</param>
        /// <param name="playlistName">The name of the playlist.</param>
        /// <param name="replaceRoles">If set to <c>true</c>, indicates current roles of the users will be replaced.</param>
        /// <returns>The constructed confirmation message.</returns>
        private string BuildConfirmationMessage(int userCount, string playlistName, bool replaceRoles, bool replaceTeams)
        {
            var message = new StringBuilder($"You are about to apply the playlist {playlistName} to {userCount} users");

            if (replaceRoles)
                message.Append(" and remove all their current roles");

            if (replaceTeams) message.Append(" and remove all their current teams");
            message.Append(".\n Are you sure?");

            return message.ToString();
        }

        public void ShowAboutDialog()
        {
            var about = new AboutDialog();
            about.ShowDialog();
        }

        private int previousUnassingedRolesList_SelectedIndex = -1;
        private int previousAssignedRolesList_SelectedIndex = -1;
        private int previousUnassignedTeamsList_SelectedIndex = -1;
        private int previousAssignedTeamsList_SelectedIndex = -1;

        private void unassingedRolesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unassingedRolesList.SelectedItems.Count == 1 && unassingedRolesList.SelectedIndex == previousUnassingedRolesList_SelectedIndex)
            {
                unassingedRolesList.SelectedIndex = -1;
            }
            previousUnassingedRolesList_SelectedIndex = unassingedRolesList.SelectedIndex;
        }

        private void assignedRolesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (assignedRolesList.SelectedItems.Count == 1 && assignedRolesList.SelectedIndex == previousAssignedRolesList_SelectedIndex)
            {
                assignedRolesList.SelectedIndex = -1;
            }
            previousAssignedRolesList_SelectedIndex = assignedRolesList.SelectedIndex;
        }

        private void unasssignedTeamsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unasssignedTeamsList.SelectedItems.Count == 1 && unasssignedTeamsList.SelectedIndex == previousUnassignedTeamsList_SelectedIndex)
            {
                unasssignedTeamsList.SelectedIndex = -1;
            }
            previousUnassignedTeamsList_SelectedIndex = unasssignedTeamsList.SelectedIndex;
        }

        private void assignedTeamsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (assignedTeamsList.SelectedItems.Count == 1 && assignedTeamsList.SelectedIndex == previousAssignedTeamsList_SelectedIndex)
            {
                assignedTeamsList.SelectedIndex = -1;
            }
            previousAssignedTeamsList_SelectedIndex = assignedTeamsList.SelectedIndex;
        }
    }
}