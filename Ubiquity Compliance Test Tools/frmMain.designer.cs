

using SKYNET.Properties;
using XNova_Utils.Others;

namespace SKYNET
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.MinBox = new System.Windows.Forms.Panel();
            this.MinPic = new System.Windows.Forms.PictureBox();
            this.CloseBox = new System.Windows.Forms.Panel();
            this.ClosePic = new System.Windows.Forms.PictureBox();
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.Descovery = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CountryCodeWorker = new System.ComponentModel.BackgroundWorker();
            this.password = new SKYNET.LoginBox();
            this.username = new SKYNET.LoginBox();
            this.serverip = new SKYNET.LoginBox();
            this.AdminDevice = new FlatButton();
            this.ActivateCT = new FlatButton();
            this.connect = new FlatButton();
            this.planetMenu = new FlatContextMenuStrip();
            this.goGalaxy = new System.Windows.Forms.ToolStripMenuItem();
            this.atacarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LaunchMisil = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.MinBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinPic)).BeginInit();
            this.CloseBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            this.WebBrowserpnl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.planetMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.MinBox);
            this.panel1.Controls.Add(this.CloseBox);
            this.panel1.Controls.Add(this.tittleLbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 26);
            this.panel1.TabIndex = 5;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
            // 
            // MinBox
            // 
            this.MinBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.MinBox.Controls.Add(this.MinPic);
            this.MinBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinBox.Location = new System.Drawing.Point(371, 0);
            this.MinBox.Name = "MinBox";
            this.MinBox.Size = new System.Drawing.Size(34, 26);
            this.MinBox.TabIndex = 12;
            this.MinBox.Click += new System.EventHandler(this.Minimize_click);
            this.MinBox.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
            this.MinBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // MinPic
            // 
            this.MinPic.Image = global::SKYNET.Properties.Resources.min_new;
            this.MinPic.Location = new System.Drawing.Point(11, 12);
            this.MinPic.Name = "MinPic";
            this.MinPic.Size = new System.Drawing.Size(13, 12);
            this.MinPic.TabIndex = 4;
            this.MinPic.TabStop = false;
            this.MinPic.Click += new System.EventHandler(this.Minimize_click);
            this.MinPic.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
            this.MinPic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // CloseBox
            // 
            this.CloseBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.CloseBox.Controls.Add(this.ClosePic);
            this.CloseBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseBox.Location = new System.Drawing.Point(405, 0);
            this.CloseBox.Name = "CloseBox";
            this.CloseBox.Size = new System.Drawing.Size(34, 26);
            this.CloseBox.TabIndex = 11;
            this.CloseBox.Click += new System.EventHandler(this.closeBox_Click);
            this.CloseBox.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
            this.CloseBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // ClosePic
            // 
            this.ClosePic.Image = global::SKYNET.Properties.Resources.close11;
            this.ClosePic.Location = new System.Drawing.Point(11, 7);
            this.ClosePic.Name = "ClosePic";
            this.ClosePic.Size = new System.Drawing.Size(13, 12);
            this.ClosePic.TabIndex = 4;
            this.ClosePic.TabStop = false;
            this.ClosePic.Click += new System.EventHandler(this.closeBox_Click);
            this.ClosePic.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
            this.ClosePic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // tittleLbl
            // 
            this.tittleLbl.AutoSize = true;
            this.tittleLbl.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tittleLbl.Location = new System.Drawing.Point(5, 4);
            this.tittleLbl.Name = "tittleLbl";
            this.tittleLbl.Size = new System.Drawing.Size(99, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Ubiquity Tools";
            this.tittleLbl.Click += new System.EventHandler(this.TittleLbl_Click);
            this.tittleLbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.tittleLbl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.tittleLbl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
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
            this.rtbLogs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.rtbLogs.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.rtbLogs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
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
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
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
            this.sip.ForeColor = System.Drawing.Color.Gainsboro;
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
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
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
            this.StatusLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.StatusLabel.Location = new System.Drawing.Point(276, 283);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(83, 16);
            this.StatusLabel.TabIndex = 48;
            this.StatusLabel.Text = "Desconectado";
            // 
            // CountryLabel
            // 
            this.CountryLabel.AutoSize = true;
            this.CountryLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.CountryLabel.Location = new System.Drawing.Point(276, 261);
            this.CountryLabel.Name = "CountryLabel";
            this.CountryLabel.Size = new System.Drawing.Size(83, 16);
            this.CountryLabel.TabIndex = 49;
            this.CountryLabel.Text = "Desconectado";
            // 
            // FirmwareLabel
            // 
            this.FirmwareLabel.AutoSize = true;
            this.FirmwareLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.FirmwareLabel.Location = new System.Drawing.Point(276, 239);
            this.FirmwareLabel.Name = "FirmwareLabel";
            this.FirmwareLabel.Size = new System.Drawing.Size(83, 16);
            this.FirmwareLabel.TabIndex = 51;
            this.FirmwareLabel.Text = "Desconectado";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.DeviceLabel.Location = new System.Drawing.Point(276, 217);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(83, 16);
            this.DeviceLabel.TabIndex = 52;
            this.DeviceLabel.Text = "Desconectado";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gainsboro;
            this.label4.Location = new System.Drawing.Point(180, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 54;
            this.label4.Text = "Dispositivo";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gainsboro;
            this.label5.Location = new System.Drawing.Point(180, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Firmware";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Gainsboro;
            this.label6.Location = new System.Drawing.Point(180, 261);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "Pais";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gainsboro;
            this.label7.Location = new System.Drawing.Point(180, 283);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 16);
            this.label7.TabIndex = 59;
            this.label7.Text = "Estado";
            // 
            // lblping
            // 
            this.lblping.AutoSize = true;
            this.lblping.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblping.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblping.Location = new System.Drawing.Point(180, 305);
            this.lblping.Name = "lblping";
            this.lblping.Size = new System.Drawing.Size(36, 16);
            this.lblping.TabIndex = 61;
            this.lblping.Text = "Ping";
            // 
            // PingLabel
            // 
            this.PingLabel.AutoSize = true;
            this.PingLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.PingLabel.Location = new System.Drawing.Point(276, 305);
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel2.Controls.Add(this.Descovery);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 416);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(439, 24);
            this.panel2.TabIndex = 64;
            // 
            // Descovery
            // 
            this.Descovery.AutoSize = true;
            this.Descovery.ForeColor = System.Drawing.Color.Gainsboro;
            this.Descovery.Location = new System.Drawing.Point(9, 2);
            this.Descovery.Name = "Descovery";
            this.Descovery.Size = new System.Drawing.Size(151, 16);
            this.Descovery.TabIndex = 54;
            this.Descovery.Text = "Descubrir equipos en la red";
            this.Descovery.Click += new System.EventHandler(this.Descovery_Click);
            this.Descovery.MouseLeave += new System.EventHandler(this.Descovery_MouseLeave);
            this.Descovery.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Descovery_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(353, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "by Hackerprod";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // CountryCodeWorker
            // 
            this.CountryCodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CountryCodeWorker_DoWork);
            // 
            // password
            // 
            this.password.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.password.Control_BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.password.Control_BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.password.Empty_Text = "Fill all data";
            this.password.ForeColor = System.Drawing.Color.Gainsboro;
            this.password.IsPassword = true;
            this.password.Location = new System.Drawing.Point(12, 327);
            this.password.Logo = global::SKYNET.Properties.Resources.key_2_60px;
            this.password.Name = "password";
            this.password.Padding = new System.Windows.Forms.Padding(2);
            this.password.ShowLogo = true;
            this.password.Size = new System.Drawing.Size(154, 37);
            this.password.TabIndex = 69;
            // 
            // username
            // 
            this.username.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.username.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.username.Control_BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.username.Control_BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.username.Empty_Text = "Fill all data";
            this.username.ForeColor = System.Drawing.Color.Gainsboro;
            this.username.IsPassword = false;
            this.username.Location = new System.Drawing.Point(12, 273);
            this.username.Logo = global::SKYNET.Properties.Resources.male_user_100px;
            this.username.Name = "username";
            this.username.Padding = new System.Windows.Forms.Padding(2);
            this.username.ShowLogo = true;
            this.username.Size = new System.Drawing.Size(154, 37);
            this.username.TabIndex = 68;
            // 
            // serverip
            // 
            this.serverip.ActivatedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.serverip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.serverip.Control_BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.serverip.Control_BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.serverip.Empty_Text = "Fill all data";
            this.serverip.ForeColor = System.Drawing.Color.Gainsboro;
            this.serverip.IsPassword = false;
            this.serverip.Location = new System.Drawing.Point(12, 218);
            this.serverip.Logo = global::SKYNET.Properties.Resources.networking_manager_100px;
            this.serverip.Name = "serverip";
            this.serverip.Padding = new System.Windows.Forms.Padding(2);
            this.serverip.ShowLogo = true;
            this.serverip.Size = new System.Drawing.Size(154, 37);
            this.serverip.TabIndex = 67;
            // 
            // AdminDevice
            // 
            this.AdminDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.AdminDevice.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.AdminDevice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AdminDevice.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AdminDevice.ForeColor = System.Drawing.Color.Gainsboro;
            this.AdminDevice.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.AdminDevice.ImageAlignment = FlatButton._ImgAlign.Left;
            this.AdminDevice.ImageIcon = null;
            this.AdminDevice.Location = new System.Drawing.Point(179, 373);
            this.AdminDevice.Name = "AdminDevice";
            this.AdminDevice.Rounded = false;
            this.AdminDevice.Size = new System.Drawing.Size(248, 29);
            this.AdminDevice.Style = FlatButton._Style.TextOnly;
            this.AdminDevice.TabIndex = 66;
            this.AdminDevice.Text = "ADMINISTRAR EQUIPO";
            this.AdminDevice.Click += new System.EventHandler(this.AdminDevice_Click);
            // 
            // ActivateCT
            // 
            this.ActivateCT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.ActivateCT.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.ActivateCT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActivateCT.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ActivateCT.ForeColor = System.Drawing.Color.Gainsboro;
            this.ActivateCT.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.ActivateCT.ImageAlignment = FlatButton._ImgAlign.Left;
            this.ActivateCT.ImageIcon = null;
            this.ActivateCT.Location = new System.Drawing.Point(179, 335);
            this.ActivateCT.Name = "ActivateCT";
            this.ActivateCT.Rounded = false;
            this.ActivateCT.Size = new System.Drawing.Size(248, 29);
            this.ActivateCT.Style = FlatButton._Style.TextOnly;
            this.ActivateCT.TabIndex = 29;
            this.ActivateCT.Text = "ACTIVAR COMPLIANCE TEST";
            this.ActivateCT.Click += new System.EventHandler(this.ActivateCT_Click);
            // 
            // connect
            // 
            this.connect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.connect.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.connect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.connect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.connect.ForeColor = System.Drawing.Color.Gainsboro;
            this.connect.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.connect.ImageAlignment = FlatButton._ImgAlign.Left;
            this.connect.ImageIcon = null;
            this.connect.Location = new System.Drawing.Point(12, 373);
            this.connect.Name = "connect";
            this.connect.Rounded = false;
            this.connect.Size = new System.Drawing.Size(154, 29);
            this.connect.Style = FlatButton._Style.TextOnly;
            this.connect.TabIndex = 28;
            this.connect.Text = "CONECTAR";
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
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(439, 440);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.serverip);
            this.Controls.Add(this.AdminDevice);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.PingLabel);
            this.Controls.Add(this.lblping);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeviceLabel);
            this.Controls.Add(this.FirmwareLabel);
            this.Controls.Add(this.CountryLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ActivateCT);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.WebBrowserpnl);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Tools";
            this.Activated += new System.EventHandler(this.FrmMain_Activated);
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.MinBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinPic)).EndInit();
            this.CloseBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            this.WebBrowserpnl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.planetMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.ImageList imageList1;
        private FlatContextMenuStrip planetMenu;
        private System.Windows.Forms.ToolStripMenuItem goGalaxy;
        private System.Windows.Forms.ToolStripMenuItem LaunchMisil;
        private System.Windows.Forms.ToolStripMenuItem atacarMenuItem;
        private System.Windows.Forms.Panel CloseBox;
        private System.Windows.Forms.PictureBox ClosePic;
        private System.Windows.Forms.Panel WebBrowserpnl;
        private System.Windows.Forms.Panel MinBox;
        private System.Windows.Forms.PictureBox MinPic;
        private FlatButton connect;
        private FlatButton ActivateCT;
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
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker CountryCodeWorker;
        private FlatButton AdminDevice;
        public System.Windows.Forms.Label StatusLabel;
        public System.Windows.Forms.Label CountryLabel;
        public System.Windows.Forms.Label FirmwareLabel;
        public System.Windows.Forms.Label DeviceLabel;
        private System.Windows.Forms.Label Descovery;
        public LoginBox serverip;
        public LoginBox username;
        public LoginBox password;
    }
}