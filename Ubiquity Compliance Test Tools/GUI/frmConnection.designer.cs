

using SKYNET.Properties;

namespace SKYNET
{
    partial class frmConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnection));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseBox = new System.Windows.Forms.Panel();
            this.ClosePic = new System.Windows.Forms.PictureBox();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DeviceContainer = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txt = new System.Windows.Forms.RichTextBox();
            this._lvAliveHosts = new System.Windows.Forms.ListView();
            this.Chann = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MAC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.signal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ccq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txpower = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.noisefloor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tx_latency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.signal_Remote = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.LoadFromFiles = new FlatButton();
            this.method1 = new FlatButton();
            this.panel1.SuspendLayout();
            this.CloseBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.DeviceContainer.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.CloseBox);
            this.panel1.Controls.Add(this.tittleLbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 26);
            this.panel1.TabIndex = 5;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
            // 
            // CloseBox
            // 
            this.CloseBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.CloseBox.Controls.Add(this.ClosePic);
            this.CloseBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseBox.Location = new System.Drawing.Point(920, 0);
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
            this.tittleLbl.Size = new System.Drawing.Size(367, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Escanear calidad de conexión de las estaciones por Canal ";
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
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(1, 491);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(954, 24);
            this.panel3.TabIndex = 65;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(954, 24);
            this.panel5.TabIndex = 66;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.progressBar1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(670, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(284, 24);
            this.panel6.TabIndex = 55;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 7);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(262, 10);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 54;
            this.progressBar1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 16);
            this.label2.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 53;
            // 
            // DeviceContainer
            // 
            this.DeviceContainer.AutoScroll = true;
            this.DeviceContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(31)))), ((int)(((byte)(34)))));
            this.DeviceContainer.Controls.Add(this.panel4);
            this.DeviceContainer.Controls.Add(this.panel2);
            this.DeviceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceContainer.Location = new System.Drawing.Point(1, 27);
            this.DeviceContainer.Name = "DeviceContainer";
            this.DeviceContainer.Size = new System.Drawing.Size(954, 464);
            this.DeviceContainer.TabIndex = 67;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txt);
            this.panel4.Controls.Add(this._lvAliveHosts);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(954, 427);
            this.panel4.TabIndex = 75;
            // 
            // txt
            // 
            this.txt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.txt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.txt.Location = new System.Drawing.Point(0, 352);
            this.txt.Name = "txt";
            this.txt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txt.Size = new System.Drawing.Size(954, 75);
            this.txt.TabIndex = 2;
            this.txt.Text = "";
            this.txt.TextChanged += new System.EventHandler(this.Txt_TextChanged);
            // 
            // _lvAliveHosts
            // 
            this._lvAliveHosts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this._lvAliveHosts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._lvAliveHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Chann,
            this.DeviceName,
            this.MAC,
            this.signal,
            this.lastip,
            this.tx,
            this.rx,
            this.ccq,
            this.txpower,
            this.noisefloor,
            this.tx_latency,
            this.signal_Remote});
            this._lvAliveHosts.Dock = System.Windows.Forms.DockStyle.Top;
            this._lvAliveHosts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this._lvAliveHosts.FullRowSelect = true;
            this._lvAliveHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvAliveHosts.HideSelection = false;
            this._lvAliveHosts.Location = new System.Drawing.Point(0, 0);
            this._lvAliveHosts.Name = "_lvAliveHosts";
            this._lvAliveHosts.Size = new System.Drawing.Size(954, 352);
            this._lvAliveHosts.TabIndex = 1;
            this._lvAliveHosts.UseCompatibleStateImageBehavior = false;
            this._lvAliveHosts.View = System.Windows.Forms.View.Details;
            this._lvAliveHosts.DoubleClick += new System.EventHandler(this._lvAliveHosts_DoubleClick);
            // 
            // Chann
            // 
            this.Chann.Text = "Channel";
            this.Chann.Width = 59;
            // 
            // DeviceName
            // 
            this.DeviceName.Text = "Device Name";
            this.DeviceName.Width = 126;
            // 
            // MAC
            // 
            this.MAC.Text = "MAC";
            this.MAC.Width = 113;
            // 
            // signal
            // 
            this.signal.Text = "Signal [Local/Remote]";
            this.signal.Width = 136;
            // 
            // lastip
            // 
            this.lastip.Text = "IP";
            this.lastip.Width = 93;
            // 
            // tx
            // 
            this.tx.Text = "TX";
            this.tx.Width = 39;
            // 
            // rx
            // 
            this.rx.Text = "RX";
            this.rx.Width = 37;
            // 
            // ccq
            // 
            this.ccq.Text = "CCQ";
            this.ccq.Width = 42;
            // 
            // txpower
            // 
            this.txpower.Text = "txpower";
            this.txpower.Width = 59;
            // 
            // noisefloor
            // 
            this.noisefloor.Text = "noisefloor";
            this.noisefloor.Width = 71;
            // 
            // tx_latency
            // 
            this.tx_latency.Text = "tx_latency";
            this.tx_latency.Width = 64;
            // 
            // signal_Remote
            // 
            this.signal_Remote.Text = "Signal [Remote]";
            this.signal_Remote.Width = 96;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.LoadFromFiles);
            this.panel2.Controls.Add(this.method1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 427);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(954, 37);
            this.panel2.TabIndex = 74;
            // 
            // LoadFromFiles
            // 
            this.LoadFromFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.LoadFromFiles.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.LoadFromFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LoadFromFiles.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LoadFromFiles.ForeColor = System.Drawing.Color.White;
            this.LoadFromFiles.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.LoadFromFiles.ImageAlignment = FlatButton._ImgAlign.Left;
            this.LoadFromFiles.ImageIcon = null;
            this.LoadFromFiles.Location = new System.Drawing.Point(682, 7);
            this.LoadFromFiles.Name = "LoadFromFiles";
            this.LoadFromFiles.Rounded = false;
            this.LoadFromFiles.Size = new System.Drawing.Size(159, 24);
            this.LoadFromFiles.Style = FlatButton._Style.TextOnly;
            this.LoadFromFiles.TabIndex = 74;
            this.LoadFromFiles.Text = "Cargar desde archivos";
            this.LoadFromFiles.Click += new System.EventHandler(this.Load_Click);
            // 
            // method1
            // 
            this.method1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.method1.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.method1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.method1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.method1.ForeColor = System.Drawing.Color.White;
            this.method1.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.method1.ImageAlignment = FlatButton._ImgAlign.Left;
            this.method1.ImageIcon = null;
            this.method1.Location = new System.Drawing.Point(847, 7);
            this.method1.Name = "method1";
            this.method1.Rounded = false;
            this.method1.Size = new System.Drawing.Size(97, 24);
            this.method1.Style = FlatButton._Style.TextOnly;
            this.method1.TabIndex = 73;
            this.method1.Text = "Buscar";
            this.method1.Click += new System.EventHandler(this.Method_Click);
            this.method1.MouseLeave += new System.EventHandler(this.Mothod_MouseLeave);
            this.method1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mothod_MouseMove);
            // 
            // frmConnection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(956, 516);
            this.Controls.Add(this.DeviceContainer);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmConnection";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.Deactivate += new System.EventHandler(this.FrmDiscovery_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConnection_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Event_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Event_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Event_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.CloseBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.DeviceContainer.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.Panel CloseBox;
        private System.Windows.Forms.PictureBox ClosePic;
        public System.Windows.Forms.Label tittleLbl;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel DeviceContainer;
        private FlatButton method1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader Chann;
        private System.Windows.Forms.ColumnHeader DeviceName;
        private System.Windows.Forms.ColumnHeader MAC;
        private System.Windows.Forms.ColumnHeader signal;
        private System.Windows.Forms.ColumnHeader lastip;
        private System.Windows.Forms.ColumnHeader tx;
        private System.Windows.Forms.ColumnHeader rx;
        private System.Windows.Forms.ColumnHeader ccq;
        private System.Windows.Forms.ColumnHeader txpower;
        private System.Windows.Forms.ColumnHeader noisefloor;
        private System.Windows.Forms.ColumnHeader tx_latency;
        private System.Windows.Forms.ColumnHeader signal_Remote;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txt;
        private FlatButton LoadFromFiles;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.ListView _lvAliveHosts;
    }
}