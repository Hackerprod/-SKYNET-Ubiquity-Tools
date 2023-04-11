

using SKYNET.GUI.Controls;
using SKYNET.Properties;

namespace SKYNET.GUI
{
    partial class frmSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch));
            this.PN_Top = new System.Windows.Forms.Panel();
            this.BT_Close = new SKYNET.Controls.SKYNET_Box();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DiscoverWorker = new System.ComponentModel.BackgroundWorker();
            this.DeviceContainer = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.MACAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SignalLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NoiceLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Quality = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.method1 = new SKYNET.GUI.Controls.SKYNET_Button();
            this.PN_Top.SuspendLayout();
            this.panel3.SuspendLayout();
            this.DeviceContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PN_Top
            // 
            this.PN_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.PN_Top.Controls.Add(this.BT_Close);
            this.PN_Top.Controls.Add(this.tittleLbl);
            this.PN_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.PN_Top.ForeColor = System.Drawing.Color.White;
            this.PN_Top.Location = new System.Drawing.Point(1, 1);
            this.PN_Top.Name = "PN_Top";
            this.PN_Top.Size = new System.Drawing.Size(437, 26);
            this.PN_Top.TabIndex = 5;
            // 
            // BT_Close
            // 
            this.BT_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Close.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_Close.Dock = System.Windows.Forms.DockStyle.Right;
            this.BT_Close.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(64)))), ((int)(((byte)(78)))));
            this.BT_Close.Image = global::SKYNET.Properties.Resources.close;
            this.BT_Close.ImageSize = 10;
            this.BT_Close.Location = new System.Drawing.Point(403, 0);
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
            this.tittleLbl.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tittleLbl.Location = new System.Drawing.Point(5, 4);
            this.tittleLbl.Name = "tittleLbl";
            this.tittleLbl.Size = new System.Drawing.Size(122, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Find access points";
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
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(1, 323);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(437, 24);
            this.panel3.TabIndex = 65;
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
            // DiscoverWorker
            // 
            this.DiscoverWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DiscoverWorker_DoWork);
            // 
            // DeviceContainer
            // 
            this.DeviceContainer.AutoScroll = true;
            this.DeviceContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(31)))), ((int)(((byte)(34)))));
            this.DeviceContainer.Controls.Add(this.textBox1);
            this.DeviceContainer.Controls.Add(this.listView1);
            this.DeviceContainer.Controls.Add(this.method1);
            this.DeviceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceContainer.Location = new System.Drawing.Point(1, 27);
            this.DeviceContainer.Name = "DeviceContainer";
            this.DeviceContainer.Size = new System.Drawing.Size(437, 296);
            this.DeviceContainer.TabIndex = 67;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 168);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(419, 92);
            this.textBox1.TabIndex = 75;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MACAddress,
            this.SignalLevel,
            this.NoiceLevel,
            this.Quality});
            this.listView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(8, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(419, 156);
            this.listView1.TabIndex = 74;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // MACAddress
            // 
            this.MACAddress.Text = "MAC Address";
            this.MACAddress.Width = 166;
            // 
            // SignalLevel
            // 
            this.SignalLevel.Text = "Signal Level";
            this.SignalLevel.Width = 78;
            // 
            // NoiceLevel
            // 
            this.NoiceLevel.Text = "Noice Level";
            this.NoiceLevel.Width = 78;
            // 
            // Quality
            // 
            this.Quality.Text = "Quality";
            this.Quality.Width = 81;
            // 
            // method1
            // 
            this.method1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
            this.method1.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.method1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.method1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.method1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.method1.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.method1.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.method1.ImageIcon = null;
            this.method1.Location = new System.Drawing.Point(330, 266);
            this.method1.MenuMode = false;
            this.method1.Name = "method1";
            this.method1.Rounded = false;
            this.method1.Size = new System.Drawing.Size(97, 24);
            this.method1.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.method1.TabIndex = 73;
            this.method1.Text = "Find";
            this.method1.Click += new System.EventHandler(this.Method_Click);
            // 
            // frmSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(164)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(439, 348);
            this.Controls.Add(this.DeviceContainer);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.PN_Top);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1440, 860);
            this.Name = "frmSearch";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.PN_Top.ResumeLayout(false);
            this.PN_Top.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.DeviceContainer.ResumeLayout(false);
            this.DeviceContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PN_Top;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        public System.Windows.Forms.Label tittleLbl;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker DiscoverWorker;
        private System.Windows.Forms.Panel DeviceContainer;
        private SKYNET_Button method1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader MACAddress;
        private System.Windows.Forms.ColumnHeader SignalLevel;
        private System.Windows.Forms.ColumnHeader NoiceLevel;
        private System.Windows.Forms.ColumnHeader Quality;
        private System.Windows.Forms.TextBox textBox1;
        private SKYNET.Controls.SKYNET_Box BT_Close;
    }
}