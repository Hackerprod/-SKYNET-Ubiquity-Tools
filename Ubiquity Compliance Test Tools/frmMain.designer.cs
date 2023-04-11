
namespace SKYNET.GUI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.PN_Top = new System.Windows.Forms.Panel();
            this.BT_Minimize = new SKYNET.Controls.SKYNET_Box();
            this.BT_Close = new SKYNET.Controls.SKYNET_Box();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.WebBrowserpnl = new System.Windows.Forms.Panel();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.CTWorker = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.sip = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IniciarSession = new System.ComponentModel.BackgroundWorker();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.CountryLabel = new System.Windows.Forms.Label();
            this.FirmwareLabel = new System.Windows.Forms.Label();
            this.DeviceLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblping = new System.Windows.Forms.Label();
            this.PingLabel = new System.Windows.Forms.Label();
            this.pingWorker = new System.ComponentModel.BackgroundWorker();
            this.CountryCodeWorker = new System.ComponentModel.BackgroundWorker();
            this.password = new SKYNET.GUI.Controls.SKYNET_TextBox();
            this.username = new SKYNET.GUI.Controls.SKYNET_TextBox();
            this.serverip = new SKYNET.GUI.Controls.SKYNET_TextBox();
            this.AdminDevice = new SKYNET.GUI.Controls.SKYNET_Button();
            this.ActivateCT = new SKYNET.GUI.Controls.SKYNET_Button();
            this.connect = new SKYNET.GUI.Controls.SKYNET_Button();
            this.planetMenu = new SKYNET.GUI.Controls.SKYNET_ContextMenuStrip();
            this.goGalaxy = new System.Windows.Forms.ToolStripMenuItem();
            this.atacarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LaunchMisil = new System.Windows.Forms.ToolStripMenuItem();
            this.SKYNET_Button1 = new SKYNET.GUI.Controls.SKYNET_Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PN_Top.SuspendLayout();
            this.WebBrowserpnl.SuspendLayout();
            this.planetMenu.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PN_Top
            // 
            this.PN_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.PN_Top.Controls.Add(this.BT_Minimize);
            this.PN_Top.Controls.Add(this.BT_Close);
            this.PN_Top.Controls.Add(this.tittleLbl);
            this.PN_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.PN_Top.ForeColor = System.Drawing.Color.White;
            this.PN_Top.Location = new System.Drawing.Point(0, 0);
            this.PN_Top.Name = "PN_Top";
            this.PN_Top.Size = new System.Drawing.Size(439, 26);
            this.PN_Top.TabIndex = 5;
            // 
            // BT_Minimize
            // 
            this.BT_Minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Minimize.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Minimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.BT_Minimize.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(64)))), ((int)(((byte)(78)))));
            this.BT_Minimize.Image = global::SKYNET.Properties.Resources.minimise;
            this.BT_Minimize.ImageSize = 10;
            this.BT_Minimize.Location = new System.Drawing.Point(371, 0);
            this.BT_Minimize.MenuMode = false;
            this.BT_Minimize.MenuSeparation = 8;
            this.BT_Minimize.Name = "BT_Minimize";
            this.BT_Minimize.Size = new System.Drawing.Size(34, 26);
            this.BT_Minimize.TabIndex = 12;
            this.BT_Minimize.BoxClicked += new System.EventHandler(this.BT_Minimize_BoxClicked);
            // 
            // BT_Close
            // 
            this.BT_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Close.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Close.Dock = System.Windows.Forms.DockStyle.Right;
            this.BT_Close.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(64)))), ((int)(((byte)(78)))));
            this.BT_Close.Image = global::SKYNET.Properties.Resources.close;
            this.BT_Close.ImageSize = 10;
            this.BT_Close.Location = new System.Drawing.Point(405, 0);
            this.BT_Close.MenuMode = false;
            this.BT_Close.MenuSeparation = 8;
            this.BT_Close.Name = "BT_Close";
            this.BT_Close.Size = new System.Drawing.Size(34, 26);
            this.BT_Close.TabIndex = 11;
            this.BT_Close.BoxClicked += new System.EventHandler(this.BT_Close_BoxClicked);
            // 
            // tittleLbl
            // 
            this.tittleLbl.AutoSize = true;
            this.tittleLbl.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.tittleLbl.Location = new System.Drawing.Point(5, 4);
            this.tittleLbl.Name = "tittleLbl";
            this.tittleLbl.Size = new System.Drawing.Size(85, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Ubiquity Tools";
            // 
            // acceptBtn
            // 
            this.acceptBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptBtn.Location = new System.Drawing.Point(819, 372);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 16;
            this.acceptBtn.Text = "button1";
            this.acceptBtn.UseVisualStyleBackColor = true;
            // 
            // Browser
            // 
            this.Browser.Location = new System.Drawing.Point(-21, -2);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(16, 20);
            this.Browser.TabIndex = 18;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "P0.jpg");
            this.imageList1.Images.SetKeyName(1, "P1.jpg");
            this.imageList1.Images.SetKeyName(2, "P2.jpg");
            this.imageList1.Images.SetKeyName(3, "P3.jpg");
            this.imageList1.Images.SetKeyName(4, "P4.jpg");
            // 
            // WebBrowserpnl
            // 
            this.WebBrowserpnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.WebBrowserpnl.Controls.Add(this.rtbLogs);
            this.WebBrowserpnl.Dock = System.Windows.Forms.DockStyle.Top;
            this.WebBrowserpnl.Location = new System.Drawing.Point(0, 26);
            this.WebBrowserpnl.Name = "WebBrowserpnl";
            this.WebBrowserpnl.Padding = new System.Windows.Forms.Padding(8);
            this.WebBrowserpnl.Size = new System.Drawing.Size(439, 175);
            this.WebBrowserpnl.TabIndex = 8;
            // 
            // rtbLogs
            // 
            this.rtbLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.rtbLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLogs.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogs.ForeColor = System.Drawing.Color.White;
            this.rtbLogs.Location = new System.Drawing.Point(8, 8);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLogs.Size = new System.Drawing.Size(423, 159);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            this.rtbLogs.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            this.rtbLogs.Enter += new System.EventHandler(this.RtbLogs_Enter);
            this.rtbLogs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtbLogs_KeyDown);
            // 
            // CTWorker
            // 
            this.CTWorker.WorkerSupportsCancellation = true;
            this.CTWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CTWorker_DoWork);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 44;
            this.label3.Text = "Password";
            // 
            // sip
            // 
            this.sip.AutoSize = true;
            this.sip.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sip.ForeColor = System.Drawing.Color.White;
            this.sip.Location = new System.Drawing.Point(9, 202);
            this.sip.Name = "sip";
            this.sip.Size = new System.Drawing.Size(53, 16);
            this.sip.TabIndex = 40;
            this.sip.Text = "Server IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "Username";
            // 
            // IniciarSession
            // 
            this.IniciarSession.DoWork += new System.ComponentModel.DoWorkEventHandler(this.IniciarSession_DoWork);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.ForeColor = System.Drawing.Color.White;
            this.StatusLabel.Location = new System.Drawing.Point(106, 83);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(83, 16);
            this.StatusLabel.TabIndex = 48;
            this.StatusLabel.Text = "Desconectado";
            // 
            // CountryLabel
            // 
            this.CountryLabel.AutoSize = true;
            this.CountryLabel.ForeColor = System.Drawing.Color.White;
            this.CountryLabel.Location = new System.Drawing.Point(106, 58);
            this.CountryLabel.Name = "CountryLabel";
            this.CountryLabel.Size = new System.Drawing.Size(83, 16);
            this.CountryLabel.TabIndex = 49;
            this.CountryLabel.Text = "Desconectado";
            // 
            // FirmwareLabel
            // 
            this.FirmwareLabel.AutoSize = true;
            this.FirmwareLabel.ForeColor = System.Drawing.Color.White;
            this.FirmwareLabel.Location = new System.Drawing.Point(106, 33);
            this.FirmwareLabel.Name = "FirmwareLabel";
            this.FirmwareLabel.Size = new System.Drawing.Size(83, 16);
            this.FirmwareLabel.TabIndex = 51;
            this.FirmwareLabel.Text = "Desconectado";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.ForeColor = System.Drawing.Color.White;
            this.DeviceLabel.Location = new System.Drawing.Point(106, 8);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(83, 16);
            this.DeviceLabel.TabIndex = 52;
            this.DeviceLabel.Text = "Desconectado";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 54;
            this.label4.Text = "Device";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Firmware";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(10, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "Country";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 16);
            this.label7.TabIndex = 59;
            this.label7.Text = "Status";
            // 
            // lblping
            // 
            this.lblping.AutoSize = true;
            this.lblping.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblping.ForeColor = System.Drawing.Color.White;
            this.lblping.Location = new System.Drawing.Point(10, 108);
            this.lblping.Name = "lblping";
            this.lblping.Size = new System.Drawing.Size(36, 16);
            this.lblping.TabIndex = 61;
            this.lblping.Text = "Ping";
            // 
            // PingLabel
            // 
            this.PingLabel.AutoSize = true;
            this.PingLabel.ForeColor = System.Drawing.Color.White;
            this.PingLabel.Location = new System.Drawing.Point(106, 108);
            this.PingLabel.Name = "PingLabel";
            this.PingLabel.Size = new System.Drawing.Size(83, 16);
            this.PingLabel.TabIndex = 62;
            this.PingLabel.Text = "Desconectado";
            // 
            // pingWorker
            // 
            this.pingWorker.WorkerSupportsCancellation = true;
            this.pingWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PingWorker_DoWork);
            // 
            // CountryCodeWorker
            // 
            this.CountryCodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CountryCodeWorker_DoWork);
            // 
            // password
            // 
            this.password.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.password.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.password.Color = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.password.ForeColor = System.Drawing.Color.White;
            this.password.IsPassword = true;
            this.password.Location = new System.Drawing.Point(12, 329);
            this.password.Logo = global::SKYNET.Properties.Resources.key_2_60px;
            this.password.LogoCursor = System.Windows.Forms.Cursors.Default;
            this.password.Name = "password";
            this.password.OnlyNumbers = false;
            this.password.Padding = new System.Windows.Forms.Padding(2);
            this.password.ShowLogo = true;
            this.password.Size = new System.Drawing.Size(154, 30);
            this.password.TabIndex = 69;
            this.password.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // username
            // 
            this.username.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.username.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.username.Color = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.username.ForeColor = System.Drawing.Color.White;
            this.username.IsPassword = false;
            this.username.Location = new System.Drawing.Point(12, 275);
            this.username.Logo = global::SKYNET.Properties.Resources.male_user_100px;
            this.username.LogoCursor = System.Windows.Forms.Cursors.Default;
            this.username.Name = "username";
            this.username.OnlyNumbers = false;
            this.username.Padding = new System.Windows.Forms.Padding(2);
            this.username.ShowLogo = true;
            this.username.Size = new System.Drawing.Size(154, 30);
            this.username.TabIndex = 68;
            this.username.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // serverip
            // 
            this.serverip.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.serverip.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.serverip.Color = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.serverip.ForeColor = System.Drawing.Color.White;
            this.serverip.IsPassword = false;
            this.serverip.Location = new System.Drawing.Point(12, 220);
            this.serverip.Logo = global::SKYNET.Properties.Resources.networking_manager_100px;
            this.serverip.LogoCursor = System.Windows.Forms.Cursors.Default;
            this.serverip.Name = "serverip";
            this.serverip.OnlyNumbers = false;
            this.serverip.Padding = new System.Windows.Forms.Padding(2);
            this.serverip.ShowLogo = true;
            this.serverip.Size = new System.Drawing.Size(154, 30);
            this.serverip.TabIndex = 67;
            this.serverip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // AdminDevice
            // 
            this.AdminDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.AdminDevice.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.AdminDevice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AdminDevice.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AdminDevice.ForeColor = System.Drawing.Color.White;
            this.AdminDevice.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.AdminDevice.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.AdminDevice.ImageIcon = null;
            this.AdminDevice.Location = new System.Drawing.Point(179, 411);
            this.AdminDevice.MenuMode = false;
            this.AdminDevice.Name = "AdminDevice";
            this.AdminDevice.Rounded = false;
            this.AdminDevice.Size = new System.Drawing.Size(248, 29);
            this.AdminDevice.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.AdminDevice.TabIndex = 66;
            this.AdminDevice.Text = "MANAGE DEVICE";
            this.AdminDevice.Click += new System.EventHandler(this.AdminDevice_Click);
            // 
            // ActivateCT
            // 
            this.ActivateCT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.ActivateCT.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.ActivateCT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActivateCT.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ActivateCT.ForeColor = System.Drawing.Color.White;
            this.ActivateCT.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.ActivateCT.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.ActivateCT.ImageIcon = null;
            this.ActivateCT.Location = new System.Drawing.Point(179, 373);
            this.ActivateCT.MenuMode = false;
            this.ActivateCT.Name = "ActivateCT";
            this.ActivateCT.Rounded = false;
            this.ActivateCT.Size = new System.Drawing.Size(248, 30);
            this.ActivateCT.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.ActivateCT.TabIndex = 29;
            this.ActivateCT.Text = "ACTIVATE COMPLIANCE TEST";
            this.ActivateCT.Click += new System.EventHandler(this.ActivateCT_Click);
            // 
            // connect
            // 
            this.connect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.connect.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.connect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.connect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.connect.ForeColor = System.Drawing.Color.White;
            this.connect.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.connect.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.connect.ImageIcon = null;
            this.connect.Location = new System.Drawing.Point(12, 373);
            this.connect.MenuMode = false;
            this.connect.Name = "connect";
            this.connect.Rounded = false;
            this.connect.Size = new System.Drawing.Size(154, 29);
            this.connect.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.connect.TabIndex = 28;
            this.connect.Text = "CONNECT";
            this.connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // planetMenu
            // 
            this.planetMenu.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.planetMenu.ForeColor = System.Drawing.Color.White;
            this.planetMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goGalaxy,
            this.atacarMenuItem,
            this.LaunchMisil});
            this.planetMenu.Name = "xnovaMenu";
            this.planetMenu.ShowImageMargin = false;
            this.planetMenu.Size = new System.Drawing.Size(120, 70);
            // 
            // goGalaxy
            // 
            this.goGalaxy.Name = "goGalaxy";
            this.goGalaxy.Size = new System.Drawing.Size(119, 22);
            this.goGalaxy.Text = "Ir a la Galaxia";
            // 
            // atacarMenuItem
            // 
            this.atacarMenuItem.Name = "atacarMenuItem";
            this.atacarMenuItem.Size = new System.Drawing.Size(119, 22);
            this.atacarMenuItem.Text = "Atacar";
            // 
            // LaunchMisil
            // 
            this.LaunchMisil.Name = "LaunchMisil";
            this.LaunchMisil.Size = new System.Drawing.Size(119, 22);
            this.LaunchMisil.Text = "Lanzar misiles";
            // 
            // SKYNET_Button1
            // 
            this.SKYNET_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.SKYNET_Button1.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.SKYNET_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SKYNET_Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SKYNET_Button1.ForeColor = System.Drawing.Color.White;
            this.SKYNET_Button1.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.SKYNET_Button1.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.SKYNET_Button1.ImageIcon = null;
            this.SKYNET_Button1.Location = new System.Drawing.Point(12, 411);
            this.SKYNET_Button1.MenuMode = false;
            this.SKYNET_Button1.Name = "SKYNET_Button1";
            this.SKYNET_Button1.Rounded = false;
            this.SKYNET_Button1.Size = new System.Drawing.Size(154, 29);
            this.SKYNET_Button1.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.SKYNET_Button1.TabIndex = 70;
            this.SKYNET_Button1.Text = "DISCOVER DEVICES";
            this.SKYNET_Button1.Click += new System.EventHandler(this.SKYNET_Button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.panel2.Controls.Add(this.lblping);
            this.panel2.Controls.Add(this.StatusLabel);
            this.panel2.Controls.Add(this.CountryLabel);
            this.panel2.Controls.Add(this.FirmwareLabel);
            this.panel2.Controls.Add(this.DeviceLabel);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.PingLabel);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(179, 220);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 139);
            this.panel2.TabIndex = 71;
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(439, 455);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SKYNET_Button1);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.serverip);
            this.Controls.Add(this.AdminDevice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ActivateCT);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.WebBrowserpnl);
            this.Controls.Add(this.PN_Top);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1440, 860);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Tools";
            this.Activated += new System.EventHandler(this.FrmMain_Activated);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.PN_Top.ResumeLayout(false);
            this.PN_Top.PerformLayout();
            this.WebBrowserpnl.ResumeLayout(false);
            this.planetMenu.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel PN_Top;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.ImageList imageList1;
        private Controls.SKYNET_ContextMenuStrip planetMenu;
        private System.Windows.Forms.ToolStripMenuItem goGalaxy;
        private System.Windows.Forms.ToolStripMenuItem LaunchMisil;
        private System.Windows.Forms.ToolStripMenuItem atacarMenuItem;
        private System.Windows.Forms.Panel WebBrowserpnl;
        private Controls.SKYNET_Button connect;
        private Controls.SKYNET_Button ActivateCT;
        public System.Windows.Forms.Label tittleLbl;
        private System.ComponentModel.BackgroundWorker CTWorker;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label sip;
        public System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker IniciarSession;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Label lblping;
        private System.Windows.Forms.Label PingLabel;
        private System.ComponentModel.BackgroundWorker pingWorker;
        private System.ComponentModel.BackgroundWorker CountryCodeWorker;
        private Controls.SKYNET_Button AdminDevice;
        public System.Windows.Forms.Label StatusLabel;
        public System.Windows.Forms.Label CountryLabel;
        public System.Windows.Forms.Label FirmwareLabel;
        public System.Windows.Forms.Label DeviceLabel;
        public Controls.SKYNET_TextBox serverip;
        public Controls.SKYNET_TextBox username;
        public Controls.SKYNET_TextBox password;
        private Controls.SKYNET_Button SKYNET_Button1;
        private System.Windows.Forms.Panel panel2;
        private SKYNET.Controls.SKYNET_Box BT_Minimize;
        private SKYNET.Controls.SKYNET_Box BT_Close;
    }
}