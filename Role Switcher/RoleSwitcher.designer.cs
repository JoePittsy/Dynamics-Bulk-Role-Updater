namespace Role_Switcher
{
    partial class RoleSwitcher
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.fetchAllUsersButton = new System.Windows.Forms.ToolStripButton();
            this.fetchXMLButton = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleLogsButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userGrid = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.playlistComboBox = new System.Windows.Forms.ComboBox();
            this.playlistBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.applyButton = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.playlistRoles = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.playlistTeams = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.savePlaylist = new System.Windows.Forms.Button();
            this.newPlaylist = new System.Windows.Forms.Button();
            this.editPlaylistComboBox = new System.Windows.Forms.ComboBox();
            this.editPlaylistName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.unassignButton = new System.Windows.Forms.Button();
            this.assignButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.unassignedRolesGroupBox = new System.Windows.Forms.GroupBox();
            this.unassingedRolesList = new System.Windows.Forms.ListBox();
            this.unassignedTeamsGroupBox = new System.Windows.Forms.GroupBox();
            this.unasssignedTeamsList = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.assignedRolesGroupBox = new System.Windows.Forms.GroupBox();
            this.assignedRolesList = new System.Windows.Forms.ListBox();
            this.assignedTeamsGroupBox = new System.Windows.Forms.GroupBox();
            this.assignedTeamsList = new System.Windows.Forms.ListBox();
            this.Logs = new System.Windows.Forms.GroupBox();
            this.logBox = new System.Windows.Forms.ListBox();
            this.logMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.removeRolesCheck = new System.Windows.Forms.CheckBox();
            this.removeTeamsCheck = new System.Windows.Forms.CheckBox();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playlistBindingSource)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.unassignedRolesGroupBox.SuspendLayout();
            this.unassignedTeamsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.assignedRolesGroupBox.SuspendLayout();
            this.assignedTeamsGroupBox.SuspendLayout();
            this.Logs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fetchAllUsersButton,
            this.fetchXMLButton,
            this.tssSeparator1,
            this.toggleLogsButton});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1397, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // fetchAllUsersButton
            // 
            this.fetchAllUsersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fetchAllUsersButton.Image = global::Role_Switcher.Properties.Resources._8080;
            this.fetchAllUsersButton.Name = "fetchAllUsersButton";
            this.fetchAllUsersButton.Size = new System.Drawing.Size(86, 22);
            this.fetchAllUsersButton.Text = "Fetch all Users";
            this.fetchAllUsersButton.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // fetchXMLButton
            // 
            this.fetchXMLButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fetchXMLButton.Name = "fetchXMLButton";
            this.fetchXMLButton.Size = new System.Drawing.Size(199, 22);
            this.fetchXMLButton.Text = "Fetch Users using FetchXML Builder";
            this.fetchXMLButton.ToolTipText = "Fetch Users using FetchXML Builder";
            this.fetchXMLButton.Click += new System.EventHandler(this.fetchXMLButton_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toggleLogsButton
            // 
            this.toggleLogsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toggleLogsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleLogsButton.Name = "toggleLogsButton";
            this.toggleLogsButton.Size = new System.Drawing.Size(101, 22);
            this.toggleLogsButton.Text = "Toggle Log Panel";
            this.toggleLogsButton.Click += new System.EventHandler(this.toggleLogsButton_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.Logs);
            this.splitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer.Size = new System.Drawing.Size(1397, 809);
            this.splitContainer.SplitterDistance = 286;
            this.splitContainer.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1397, 809);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1389, 783);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Assign Roles";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(1383, 777);
            this.splitContainer2.SplitterDistance = 828;
            this.splitContainer2.TabIndex = 1;
            // 
            // userGrid
            // 
            this.userGrid.AllowUserToOrderColumns = true;
            this.userGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGrid.Location = new System.Drawing.Point(0, 0);
            this.userGrid.Name = "userGrid";
            this.userGrid.ReadOnly = true;
            this.userGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.userGrid.Size = new System.Drawing.Size(828, 777);
            this.userGrid.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(551, 777);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.playlistComboBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlists";
            // 
            // playlistComboBox
            // 
            this.playlistComboBox.DataSource = this.playlistBindingSource;
            this.playlistComboBox.DisplayMember = "Name";
            this.playlistComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistComboBox.FormattingEnabled = true;
            this.playlistComboBox.Location = new System.Drawing.Point(3, 16);
            this.playlistComboBox.Name = "playlistComboBox";
            this.playlistComboBox.Size = new System.Drawing.Size(539, 21);
            this.playlistComboBox.TabIndex = 0;
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // playlistBindingSource
            // 
            this.playlistBindingSource.DataSource = typeof(Role_Switcher.Playlist);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.removeTeamsCheck, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.applyButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.removeRolesCheck, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 698);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(545, 76);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // applyButton
            // 
            this.applyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyButton.Enabled = false;
            this.applyButton.Location = new System.Drawing.Point(275, 3);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(267, 28);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply Playlist";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 53);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(545, 639);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.playlistRoles);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(537, 613);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Roles to Apply";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // playlistRoles
            // 
            this.playlistRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistRoles.FormattingEnabled = true;
            this.playlistRoles.Location = new System.Drawing.Point(3, 3);
            this.playlistRoles.Name = "playlistRoles";
            this.playlistRoles.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.playlistRoles.Size = new System.Drawing.Size(531, 607);
            this.playlistRoles.Sorted = true;
            this.playlistRoles.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.playlistTeams);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(339, 358);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Teams to Join";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // playlistTeams
            // 
            this.playlistTeams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistTeams.FormattingEnabled = true;
            this.playlistTeams.Location = new System.Drawing.Point(3, 3);
            this.playlistTeams.Name = "playlistTeams";
            this.playlistTeams.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.playlistTeams.Size = new System.Drawing.Size(333, 352);
            this.playlistTeams.Sorted = true;
            this.playlistTeams.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(897, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edit Role Sets";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.deleteButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.splitContainer3, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(891, 480);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(402, 74);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Role Sets";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.savePlaylist, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.newPlaylist, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.editPlaylistComboBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.editPlaylistName, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(396, 55);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // savePlaylist
            // 
            this.savePlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savePlaylist.Location = new System.Drawing.Point(300, 30);
            this.savePlaylist.Name = "savePlaylist";
            this.savePlaylist.Size = new System.Drawing.Size(93, 22);
            this.savePlaylist.TabIndex = 0;
            this.savePlaylist.Text = "Save Role Set";
            this.savePlaylist.UseVisualStyleBackColor = true;
            this.savePlaylist.Click += new System.EventHandler(this.savePlaylist_Click);
            // 
            // newPlaylist
            // 
            this.newPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newPlaylist.Location = new System.Drawing.Point(300, 3);
            this.newPlaylist.Name = "newPlaylist";
            this.newPlaylist.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.newPlaylist.Size = new System.Drawing.Size(93, 21);
            this.newPlaylist.TabIndex = 1;
            this.newPlaylist.Text = "New Role Set";
            this.newPlaylist.UseVisualStyleBackColor = true;
            this.newPlaylist.Click += new System.EventHandler(this.newPlaylist_Click);
            // 
            // editPlaylistComboBox
            // 
            this.editPlaylistComboBox.DataSource = this.playlistBindingSource;
            this.editPlaylistComboBox.DisplayMember = "Name";
            this.editPlaylistComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.editPlaylistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editPlaylistComboBox.FormattingEnabled = true;
            this.editPlaylistComboBox.Location = new System.Drawing.Point(3, 3);
            this.editPlaylistComboBox.Name = "editPlaylistComboBox";
            this.editPlaylistComboBox.Size = new System.Drawing.Size(291, 21);
            this.editPlaylistComboBox.TabIndex = 2;
            this.editPlaylistComboBox.SelectedIndexChanged += new System.EventHandler(this.editPlaylistComboBox_SelectedIndexChanged);
            // 
            // editPlaylistName
            // 
            this.editPlaylistName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.editPlaylistName.Location = new System.Drawing.Point(3, 32);
            this.editPlaylistName.Name = "editPlaylistName";
            this.editPlaylistName.Size = new System.Drawing.Size(291, 20);
            this.editPlaylistName.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.unassignButton);
            this.panel1.Controls.Add(this.assignButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(411, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(69, 394);
            this.panel1.TabIndex = 6;
            // 
            // unassignButton
            // 
            this.unassignButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.unassignButton.Location = new System.Drawing.Point(0, 23);
            this.unassignButton.Name = "unassignButton";
            this.unassignButton.Size = new System.Drawing.Size(69, 23);
            this.unassignButton.TabIndex = 8;
            this.unassignButton.Text = "<<<";
            this.unassignButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.unassignButton.UseVisualStyleBackColor = true;
            this.unassignButton.Click += new System.EventHandler(this.unassignButton_Click);
            // 
            // assignButton
            // 
            this.assignButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.assignButton.Location = new System.Drawing.Point(0, 0);
            this.assignButton.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.assignButton.Name = "assignButton";
            this.assignButton.Size = new System.Drawing.Size(69, 23);
            this.assignButton.TabIndex = 7;
            this.assignButton.Text = ">>>";
            this.assignButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.assignButton.UseVisualStyleBackColor = true;
            this.assignButton.Click += new System.EventHandler(this.assignButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.deleteButton.Location = new System.Drawing.Point(486, 57);
            this.deleteButton.MaximumSize = new System.Drawing.Size(200, 20);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.deleteButton.Size = new System.Drawing.Size(200, 20);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "DELETE ROLE SET";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 83);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.unassignedRolesGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.unassignedTeamsGroupBox);
            this.splitContainer1.Size = new System.Drawing.Size(402, 394);
            this.splitContainer1.SplitterDistance = 134;
            this.splitContainer1.TabIndex = 8;
            // 
            // unassignedRolesGroupBox
            // 
            this.unassignedRolesGroupBox.Controls.Add(this.unassingedRolesList);
            this.unassignedRolesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unassignedRolesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.unassignedRolesGroupBox.Name = "unassignedRolesGroupBox";
            this.unassignedRolesGroupBox.Size = new System.Drawing.Size(402, 134);
            this.unassignedRolesGroupBox.TabIndex = 0;
            this.unassignedRolesGroupBox.TabStop = false;
            this.unassignedRolesGroupBox.Text = "Unassigned Roles";
            // 
            // unassingedRolesList
            // 
            this.unassingedRolesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unassingedRolesList.FormattingEnabled = true;
            this.unassingedRolesList.Location = new System.Drawing.Point(3, 16);
            this.unassingedRolesList.Name = "unassingedRolesList";
            this.unassingedRolesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.unassingedRolesList.Size = new System.Drawing.Size(396, 115);
            this.unassingedRolesList.Sorted = true;
            this.unassingedRolesList.TabIndex = 1;
            this.unassingedRolesList.SelectedIndexChanged += new System.EventHandler(this.unassingedRolesList_SelectedIndexChanged);
            // 
            // unassignedTeamsGroupBox
            // 
            this.unassignedTeamsGroupBox.Controls.Add(this.unasssignedTeamsList);
            this.unassignedTeamsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unassignedTeamsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.unassignedTeamsGroupBox.Name = "unassignedTeamsGroupBox";
            this.unassignedTeamsGroupBox.Size = new System.Drawing.Size(402, 256);
            this.unassignedTeamsGroupBox.TabIndex = 0;
            this.unassignedTeamsGroupBox.TabStop = false;
            this.unassignedTeamsGroupBox.Text = "Unassigned Teams";
            // 
            // unasssignedTeamsList
            // 
            this.unasssignedTeamsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unasssignedTeamsList.FormattingEnabled = true;
            this.unasssignedTeamsList.Location = new System.Drawing.Point(3, 16);
            this.unasssignedTeamsList.Name = "unasssignedTeamsList";
            this.unasssignedTeamsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.unasssignedTeamsList.Size = new System.Drawing.Size(396, 237);
            this.unasssignedTeamsList.Sorted = true;
            this.unasssignedTeamsList.TabIndex = 0;
            this.unasssignedTeamsList.SelectedIndexChanged += new System.EventHandler(this.unasssignedTeamsList_SelectedIndexChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(486, 83);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.assignedRolesGroupBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.assignedTeamsGroupBox);
            this.splitContainer3.Size = new System.Drawing.Size(402, 394);
            this.splitContainer3.SplitterDistance = 134;
            this.splitContainer3.TabIndex = 9;
            // 
            // assignedRolesGroupBox
            // 
            this.assignedRolesGroupBox.Controls.Add(this.assignedRolesList);
            this.assignedRolesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assignedRolesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.assignedRolesGroupBox.Name = "assignedRolesGroupBox";
            this.assignedRolesGroupBox.Size = new System.Drawing.Size(402, 134);
            this.assignedRolesGroupBox.TabIndex = 0;
            this.assignedRolesGroupBox.TabStop = false;
            this.assignedRolesGroupBox.Text = "Assigned Roles";
            // 
            // assignedRolesList
            // 
            this.assignedRolesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assignedRolesList.FormattingEnabled = true;
            this.assignedRolesList.Location = new System.Drawing.Point(3, 16);
            this.assignedRolesList.Name = "assignedRolesList";
            this.assignedRolesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.assignedRolesList.Size = new System.Drawing.Size(396, 115);
            this.assignedRolesList.Sorted = true;
            this.assignedRolesList.TabIndex = 1;
            this.assignedRolesList.SelectedIndexChanged += new System.EventHandler(this.assignedRolesList_SelectedIndexChanged);
            // 
            // assignedTeamsGroupBox
            // 
            this.assignedTeamsGroupBox.Controls.Add(this.assignedTeamsList);
            this.assignedTeamsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assignedTeamsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.assignedTeamsGroupBox.Name = "assignedTeamsGroupBox";
            this.assignedTeamsGroupBox.Size = new System.Drawing.Size(402, 256);
            this.assignedTeamsGroupBox.TabIndex = 0;
            this.assignedTeamsGroupBox.TabStop = false;
            this.assignedTeamsGroupBox.Text = "Assigned Teams";
            // 
            // assignedTeamsList
            // 
            this.assignedTeamsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assignedTeamsList.FormattingEnabled = true;
            this.assignedTeamsList.Location = new System.Drawing.Point(3, 16);
            this.assignedTeamsList.Name = "assignedTeamsList";
            this.assignedTeamsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.assignedTeamsList.Size = new System.Drawing.Size(396, 237);
            this.assignedTeamsList.Sorted = true;
            this.assignedTeamsList.TabIndex = 0;
            this.assignedTeamsList.SelectedIndexChanged += new System.EventHandler(this.assignedTeamsList_SelectedIndexChanged);
            // 
            // Logs
            // 
            this.Logs.AutoSize = true;
            this.Logs.Controls.Add(this.logBox);
            this.Logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logs.Location = new System.Drawing.Point(0, 0);
            this.Logs.Name = "Logs";
            this.Logs.Size = new System.Drawing.Size(150, 46);
            this.Logs.TabIndex = 0;
            this.Logs.TabStop = false;
            this.Logs.Text = "Logs";
            // 
            // logBox
            // 
            this.logBox.DataSource = this.logMessageBindingSource;
            this.logBox.DisplayMember = "Message";
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.FormattingEnabled = true;
            this.logBox.Location = new System.Drawing.Point(3, 16);
            this.logBox.Name = "logBox";
            this.logBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.logBox.Size = new System.Drawing.Size(144, 27);
            this.logBox.TabIndex = 0;
            // 
            // logMessageBindingSource
            // 
            this.logMessageBindingSource.DataSource = typeof(Role_Switcher.LogMessage);
            // 
            // removeRolesCheck
            // 
            this.removeRolesCheck.AutoSize = true;
            this.removeRolesCheck.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.removeRolesCheck.Location = new System.Drawing.Point(3, 14);
            this.removeRolesCheck.Name = "removeRolesCheck";
            this.removeRolesCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.removeRolesCheck.Size = new System.Drawing.Size(266, 17);
            this.removeRolesCheck.TabIndex = 1;
            this.removeRolesCheck.Text = "Remove users existing roles";
            this.removeRolesCheck.UseVisualStyleBackColor = true;
            // 
            // removeTeamsCheck
            // 
            this.removeTeamsCheck.AutoSize = true;
            this.removeTeamsCheck.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.removeTeamsCheck.Location = new System.Drawing.Point(3, 56);
            this.removeTeamsCheck.Name = "removeTeamsCheck";
            this.removeTeamsCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.removeTeamsCheck.Size = new System.Drawing.Size(266, 17);
            this.removeTeamsCheck.TabIndex = 2;
            this.removeTeamsCheck.Text = "Remove users existing teams";
            this.removeTeamsCheck.UseVisualStyleBackColor = true;
            // 
            // RoleSwitcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "RoleSwitcher";
            this.Size = new System.Drawing.Size(1397, 834);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playlistBindingSource)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.unassignedRolesGroupBox.ResumeLayout(false);
            this.unassignedTeamsGroupBox.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.assignedRolesGroupBox.ResumeLayout(false);
            this.assignedTeamsGroupBox.ResumeLayout(false);
            this.Logs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton fetchAllUsersButton;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox Logs;
        private System.Windows.Forms.ListBox logBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.DataGridView userGrid;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripButton fetchXMLButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox playlistComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.ToolStripButton toggleLogsButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button savePlaylist;
        private System.Windows.Forms.Button newPlaylist;
        private System.Windows.Forms.ComboBox editPlaylistComboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button unassignButton;
        private System.Windows.Forms.Button assignButton;
        private System.Windows.Forms.BindingSource playlistBindingSource;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.BindingSource logMessageBindingSource;
        private System.Windows.Forms.TextBox editPlaylistName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox unassignedRolesGroupBox;
        private System.Windows.Forms.ListBox unassingedRolesList;
        private System.Windows.Forms.GroupBox unassignedTeamsGroupBox;
        private System.Windows.Forms.ListBox unasssignedTeamsList;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox assignedRolesGroupBox;
        private System.Windows.Forms.ListBox assignedRolesList;
        private System.Windows.Forms.GroupBox assignedTeamsGroupBox;
        private System.Windows.Forms.ListBox assignedTeamsList;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox playlistRoles;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListBox playlistTeams;
        private System.Windows.Forms.CheckBox removeTeamsCheck;
        private System.Windows.Forms.CheckBox removeRolesCheck;
    }
}
