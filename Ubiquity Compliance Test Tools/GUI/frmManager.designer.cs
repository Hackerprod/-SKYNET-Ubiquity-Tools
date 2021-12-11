

using SKYNET.Properties;

namespace SKYNET
{
    partial class frmManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.MinBox = new System.Windows.Forms.Panel();
            this.MinPic = new System.Windows.Forms.PictureBox();
            this.CloseBox = new System.Windows.Forms.Panel();
            this.ClosePic = new System.Windows.Forms.PictureBox();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.WebBrowserpnl = new System.Windows.Forms.Panel();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.CurrentPower = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.DeviceLogo = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.Stations = new FlatButton();
            this.Buscar = new FlatButton();
            this.username = new SKYNET.LoginBox();
            this.Cambiar = new FlatButton();
            this.addCT = new FlatButton();
            this.RebootDevice = new FlatButton();
            this.SetChannel = new FlatButton();
            this.PowerBar = new HackerProd.Controles.HackTrackBar();
            this.SetPower = new FlatButton();
            this.panel8 = new System.Windows.Forms.Panel();
            this.ChannelList = new FlatComboBox();
            this.panel1.SuspendLayout();
            this.MinBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinPic)).BeginInit();
            this.CloseBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            this.WebBrowserpnl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceLogo)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(618, 26);
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
            this.MinBox.Location = new System.Drawing.Point(550, 0);
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
            this.CloseBox.Location = new System.Drawing.Point(584, 0);
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
            this.tittleLbl.Size = new System.Drawing.Size(198, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Administrar ajustes del equipo";
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
            // WebBrowserpnl
            // 
            this.WebBrowserpnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.WebBrowserpnl.Controls.Add(this.rtbLogs);
            this.WebBrowserpnl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.WebBrowserpnl.Location = new System.Drawing.Point(0, 365);
            this.WebBrowserpnl.Name = "WebBrowserpnl";
            this.WebBrowserpnl.Padding = new System.Windows.Forms.Padding(8);
            this.WebBrowserpnl.Size = new System.Drawing.Size(618, 85);
            this.WebBrowserpnl.TabIndex = 8;
            // 
            // rtbLogs
            // 
            this.rtbLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.rtbLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLogs.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.rtbLogs.Location = new System.Drawing.Point(8, 8);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLogs.Size = new System.Drawing.Size(602, 69);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            this.rtbLogs.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            this.rtbLogs.Enter += new System.EventHandler(this.RtbLogs_Enter);
            this.rtbLogs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtbLogs_KeyDown);
            this.rtbLogs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.rtbLogs.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.rtbLogs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StatusLabel.Location = new System.Drawing.Point(108, 66);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(83, 16);
            this.StatusLabel.TabIndex = 48;
            this.StatusLabel.Text = "Desconectado";
            // 
            // CountryLabel
            // 
            this.CountryLabel.AutoSize = true;
            this.CountryLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.CountryLabel.Location = new System.Drawing.Point(108, 47);
            this.CountryLabel.Name = "CountryLabel";
            this.CountryLabel.Size = new System.Drawing.Size(83, 16);
            this.CountryLabel.TabIndex = 49;
            this.CountryLabel.Text = "Desconectado";
            // 
            // FirmwareLabel
            // 
            this.FirmwareLabel.AutoSize = true;
            this.FirmwareLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FirmwareLabel.Location = new System.Drawing.Point(108, 28);
            this.FirmwareLabel.Name = "FirmwareLabel";
            this.FirmwareLabel.Size = new System.Drawing.Size(83, 16);
            this.FirmwareLabel.TabIndex = 51;
            this.FirmwareLabel.Text = "Desconectado";
            // 
            // DeviceLabel
            // 
            this.DeviceLabel.AutoSize = true;
            this.DeviceLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DeviceLabel.Location = new System.Drawing.Point(108, 10);
            this.DeviceLabel.Name = "DeviceLabel";
            this.DeviceLabel.Size = new System.Drawing.Size(83, 16);
            this.DeviceLabel.TabIndex = 52;
            this.DeviceLabel.Text = "Desconectado";
            this.DeviceLabel.TextChanged += new System.EventHandler(this.DeviceLabel_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 54;
            this.label4.Text = "Dispositivo";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(12, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Firmware";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(12, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "Pais";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(12, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 16);
            this.label7.TabIndex = 59;
            this.label7.Text = "Estado";
            // 
            // lblping
            // 
            this.lblping.AutoSize = true;
            this.lblping.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblping.ForeColor = System.Drawing.Color.LightGray;
            this.lblping.Location = new System.Drawing.Point(12, 85);
            this.lblping.Name = "lblping";
            this.lblping.Size = new System.Drawing.Size(36, 16);
            this.lblping.TabIndex = 61;
            this.lblping.Text = "Ping";
            // 
            // PingLabel
            // 
            this.PingLabel.AutoSize = true;
            this.PingLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.PingLabel.Location = new System.Drawing.Point(108, 85);
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
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.PowerBar);
            this.panel2.Controls.Add(this.SetPower);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 217);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(305, 67);
            this.panel2.TabIndex = 67;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel5.Controls.Add(this.CurrentPower);
            this.panel5.Location = new System.Drawing.Point(182, 29);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(29, 24);
            this.panel5.TabIndex = 70;
            // 
            // CurrentPower
            // 
            this.CurrentPower.AutoSize = true;
            this.CurrentPower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.CurrentPower.Location = new System.Drawing.Point(5, 4);
            this.CurrentPower.Name = "CurrentPower";
            this.CurrentPower.Size = new System.Drawing.Size(20, 16);
            this.CurrentPower.TabIndex = 70;
            this.CurrentPower.Text = "20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 16);
            this.label1.TabIndex = 68;
            this.label1.Text = "AJUSTAR POTENCIA";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.SetChannel);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Location = new System.Drawing.Point(12, 134);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(305, 67);
            this.panel4.TabIndex = 69;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(5, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 16);
            this.label3.TabIndex = 68;
            this.label3.Text = "LISTA DE FRECUENCIAS";
            // 
            // DeviceLogo
            // 
            this.DeviceLogo.Location = new System.Drawing.Point(290, 8);
            this.DeviceLogo.Name = "DeviceLogo";
            this.DeviceLogo.Size = new System.Drawing.Size(88, 88);
            this.DeviceLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DeviceLogo.TabIndex = 70;
            this.DeviceLogo.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.username);
            this.panel3.Controls.Add(this.Cambiar);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(323, 134);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(287, 67);
            this.panel3.TabIndex = 74;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 16);
            this.label2.TabIndex = 68;
            this.label2.Text = "CAMBIAR NOMBRE DE USUARIO";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.LightGray;
            this.label9.Location = new System.Drawing.Point(9, 310);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(268, 16);
            this.label9.TabIndex = 76;
            this.label9.Text = "No aparece Compliance Test en la lista de paises?";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.LightGray;
            this.label8.Location = new System.Drawing.Point(5, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(259, 16);
            this.label8.TabIndex = 68;
            this.label8.Text = "MANEJO DE LAS ESTACIONES [SOLO AP]";
            this.label8.Click += new System.EventHandler(this.Label8_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel6.Controls.Add(this.Stations);
            this.panel6.Controls.Add(this.Buscar);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Location = new System.Drawing.Point(323, 217);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(287, 67);
            this.panel6.TabIndex = 75;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.StatusLabel);
            this.panel7.Controls.Add(this.CountryLabel);
            this.panel7.Controls.Add(this.FirmwareLabel);
            this.panel7.Controls.Add(this.DeviceLabel);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.DeviceLogo);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.lblping);
            this.panel7.Controls.Add(this.PingLabel);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 26);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(618, 105);
            this.panel7.TabIndex = 77;
            // 
            // Stations
            // 
            this.Stations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.Stations.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.Stations.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Stations.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Stations.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.Stations.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.Stations.ImageAlignment = FlatButton._ImgAlign.Left;
            this.Stations.ImageIcon = null;
            this.Stations.Location = new System.Drawing.Point(8, 29);
            this.Stations.Name = "Stations";
            this.Stations.Rounded = false;
            this.Stations.Size = new System.Drawing.Size(125, 24);
            this.Stations.Style = FlatButton._Style.TextOnly;
            this.Stations.TabIndex = 70;
            this.Stations.Text = "Mostrar enlaces";
            this.Stations.Click += new System.EventHandler(this.Stations_Click);
            // 
            // Buscar
            // 
            this.Buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.Buscar.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.Buscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Buscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Buscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.Buscar.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.Buscar.ImageAlignment = FlatButton._ImgAlign.Left;
            this.Buscar.ImageIcon = null;
            this.Buscar.Location = new System.Drawing.Point(154, 29);
            this.Buscar.Name = "Buscar";
            this.Buscar.Rounded = false;
            this.Buscar.Size = new System.Drawing.Size(125, 24);
            this.Buscar.Style = FlatButton._Style.TextOnly;
            this.Buscar.TabIndex = 69;
            this.Buscar.Text = "Escanear canales";
            this.Buscar.Click += new System.EventHandler(this.Buscar_Click);
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
            this.username.Location = new System.Drawing.Point(8, 25);
            this.username.Logo = global::SKYNET.Properties.Resources.male_user_100px;
            this.username.Name = "username";
            this.username.Padding = new System.Windows.Forms.Padding(2);
            this.username.ShowLogo = true;
            this.username.Size = new System.Drawing.Size(188, 30);
            this.username.TabIndex = 69;
            // 
            // Cambiar
            // 
            this.Cambiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.Cambiar.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.Cambiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Cambiar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Cambiar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.Cambiar.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.Cambiar.ImageAlignment = FlatButton._ImgAlign.Left;
            this.Cambiar.ImageIcon = null;
            this.Cambiar.Location = new System.Drawing.Point(202, 25);
            this.Cambiar.Name = "Cambiar";
            this.Cambiar.Rounded = false;
            this.Cambiar.Size = new System.Drawing.Size(77, 30);
            this.Cambiar.Style = FlatButton._Style.TextOnly;
            this.Cambiar.TabIndex = 68;
            this.Cambiar.Text = "Cambiar";
            this.Cambiar.Click += new System.EventHandler(this.Cambiar_Click);
            // 
            // addCT
            // 
            this.addCT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.addCT.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.addCT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addCT.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.addCT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.addCT.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.addCT.ImageAlignment = FlatButton._ImgAlign.Left;
            this.addCT.ImageIcon = null;
            this.addCT.Location = new System.Drawing.Point(12, 331);
            this.addCT.Name = "addCT";
            this.addCT.Rounded = false;
            this.addCT.Size = new System.Drawing.Size(176, 24);
            this.addCT.Style = FlatButton._Style.TextOnly;
            this.addCT.TabIndex = 73;
            this.addCT.Text = "Agregar Compliance Test";
            this.addCT.Click += new System.EventHandler(this.AddCT_Click);
            // 
            // RebootDevice
            // 
            this.RebootDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.RebootDevice.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.RebootDevice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RebootDevice.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RebootDevice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.RebootDevice.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.RebootDevice.ImageAlignment = FlatButton._ImgAlign.Left;
            this.RebootDevice.ImageIcon = null;
            this.RebootDevice.Location = new System.Drawing.Point(451, 331);
            this.RebootDevice.Name = "RebootDevice";
            this.RebootDevice.Rounded = false;
            this.RebootDevice.Size = new System.Drawing.Size(159, 24);
            this.RebootDevice.Style = FlatButton._Style.TextOnly;
            this.RebootDevice.TabIndex = 72;
            this.RebootDevice.Text = "Reiniciar dispositivo";
            this.RebootDevice.Click += new System.EventHandler(this.RebootDevice_Click);
            // 
            // SetChannel
            // 
            this.SetChannel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.SetChannel.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.SetChannel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetChannel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SetChannel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.SetChannel.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.SetChannel.ImageAlignment = FlatButton._ImgAlign.Left;
            this.SetChannel.ImageIcon = null;
            this.SetChannel.Location = new System.Drawing.Point(217, 24);
            this.SetChannel.Name = "SetChannel";
            this.SetChannel.Rounded = false;
            this.SetChannel.Size = new System.Drawing.Size(77, 31);
            this.SetChannel.Style = FlatButton._Style.TextOnly;
            this.SetChannel.TabIndex = 68;
            this.SetChannel.Text = "Ajustar";
            this.SetChannel.Click += new System.EventHandler(this.SetChannel_Click);
            // 
            // PowerBar
            // 
            this.PowerBar.DrawValueString = false;
            this.PowerBar.JumpToMouse = false;
            this.PowerBar.Location = new System.Drawing.Point(8, 31);
            this.PowerBar.Maximum = 10;
            this.PowerBar.Minimum = 0;
            this.PowerBar.MinimumSize = new System.Drawing.Size(47, 22);
            this.PowerBar.Name = "PowerBar";
            this.PowerBar.Size = new System.Drawing.Size(168, 22);
            this.PowerBar.TabIndex = 68;
            this.PowerBar.Value = 0;
            this.PowerBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PowerBar_MouseMove);
            // 
            // SetPower
            // 
            this.SetPower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(61)))), ((int)(((byte)(75)))));
            this.SetPower.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.SetPower.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetPower.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SetPower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.SetPower.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.SetPower.ImageAlignment = FlatButton._ImgAlign.Left;
            this.SetPower.ImageIcon = null;
            this.SetPower.Location = new System.Drawing.Point(217, 29);
            this.SetPower.Name = "SetPower";
            this.SetPower.Rounded = false;
            this.SetPower.Size = new System.Drawing.Size(77, 24);
            this.SetPower.Style = FlatButton._Style.TextOnly;
            this.SetPower.TabIndex = 68;
            this.SetPower.Text = "Ajustar";
            this.SetPower.Click += new System.EventHandler(this.SetPower_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel8.Controls.Add(this.ChannelList);
            this.panel8.Location = new System.Drawing.Point(8, 25);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(203, 30);
            this.panel8.TabIndex = 72;
            // 
            // ChannelList
            // 
            this.ChannelList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.ChannelList.BackColorMouseOver = System.Drawing.Color.Empty;
            this.ChannelList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChannelList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ChannelList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelList.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChannelList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.ChannelList.FormattingEnabled = true;
            this.ChannelList.ItemHeight = 18;
            this.ChannelList.Location = new System.Drawing.Point(-4, 3);
            this.ChannelList.Name = "ChannelList";
            this.ChannelList.Size = new System.Drawing.Size(203, 24);
            this.ChannelList.TabIndex = 35;
            // 
            // frmManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(618, 450);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.addCT);
            this.Controls.Add(this.RebootDevice);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.WebBrowserpnl);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmManager_FormClosing);
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
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceLogo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.Panel CloseBox;
        private System.Windows.Forms.PictureBox ClosePic;
        private System.Windows.Forms.Panel WebBrowserpnl;
        private System.Windows.Forms.Panel MinBox;
        private System.Windows.Forms.PictureBox MinPic;
        public System.Windows.Forms.Label tittleLbl;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label CountryLabel;
        private System.Windows.Forms.Label FirmwareLabel;
        private System.Windows.Forms.Label DeviceLabel;
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
        private FlatButton SetPower;
        private HackerProd.Controles.HackTrackBar PowerBar;
        private System.Windows.Forms.Panel panel4;
        private FlatButton SetChannel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label CurrentPower;
        private System.Windows.Forms.PictureBox DeviceLogo;
        private FlatButton RebootDevice;
        private FlatButton addCT;
        private System.Windows.Forms.Panel panel3;
        private FlatButton Cambiar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel6;
        private FlatButton Buscar;
        private FlatButton Stations;
        private System.Windows.Forms.Panel panel7;
        public LoginBox username;
        private System.Windows.Forms.Panel panel8;
        private FlatComboBox ChannelList;
    }
}