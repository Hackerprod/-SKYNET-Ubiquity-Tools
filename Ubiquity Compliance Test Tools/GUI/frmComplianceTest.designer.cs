

using SKYNET.Properties;

namespace SKYNET
{
    partial class frmComplianceTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComplianceTest));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseBox = new System.Windows.Forms.Panel();
            this.ClosePic = new System.Windows.Forms.PictureBox();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DiscoverWorker = new System.ComponentModel.BackgroundWorker();
            this.DeviceContainer = new System.Windows.Forms.Panel();
            this.aboutPanel = new System.Windows.Forms.Panel();
            this.about = new System.Windows.Forms.Label();
            this.method2 = new FlatButton();
            this.method1 = new FlatButton();
            this.panel1.SuspendLayout();
            this.CloseBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePic)).BeginInit();
            this.panel3.SuspendLayout();
            this.DeviceContainer.SuspendLayout();
            this.aboutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.CloseBox);
            this.panel1.Controls.Add(this.tittleLbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(437, 26);
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
            this.CloseBox.Location = new System.Drawing.Point(403, 0);
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
            this.tittleLbl.ForeColor = System.Drawing.Color.White;
            this.tittleLbl.Location = new System.Drawing.Point(5, 4);
            this.tittleLbl.Name = "tittleLbl";
            this.tittleLbl.Size = new System.Drawing.Size(255, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Define the method to apply to the team";
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
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(1, 117);
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
            this.DeviceContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.DeviceContainer.Controls.Add(this.aboutPanel);
            this.DeviceContainer.Controls.Add(this.method2);
            this.DeviceContainer.Controls.Add(this.method1);
            this.DeviceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceContainer.Location = new System.Drawing.Point(1, 27);
            this.DeviceContainer.Name = "DeviceContainer";
            this.DeviceContainer.Size = new System.Drawing.Size(437, 90);
            this.DeviceContainer.TabIndex = 67;
            // 
            // aboutPanel
            // 
            this.aboutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.aboutPanel.Controls.Add(this.about);
            this.aboutPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.aboutPanel.Location = new System.Drawing.Point(171, 15);
            this.aboutPanel.Name = "aboutPanel";
            this.aboutPanel.Size = new System.Drawing.Size(206, 24);
            this.aboutPanel.TabIndex = 75;
            this.aboutPanel.Visible = false;
            // 
            // about
            // 
            this.about.AutoSize = true;
            this.about.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.about.ForeColor = System.Drawing.Color.White;
            this.about.Location = new System.Drawing.Point(3, 3);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(187, 16);
            this.about.TabIndex = 0;
            this.about.Text = "Basic method. No reboot required";
            // 
            // method2
            // 
            this.method2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.method2.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.method2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.method2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.method2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.method2.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.method2.ImageAlignment = FlatButton._ImgAlign.Left;
            this.method2.ImageIcon = null;
            this.method2.Location = new System.Drawing.Point(12, 50);
            this.method2.Name = "method2";
            this.method2.Rounded = false;
            this.method2.Size = new System.Drawing.Size(141, 24);
            this.method2.Style = FlatButton._Style.TextOnly;
            this.method2.TabIndex = 74;
            this.method2.Text = "Advanced method";
            this.method2.Click += new System.EventHandler(this.Method_Click);
            this.method2.MouseLeave += new System.EventHandler(this.Mothod_MouseLeave);
            this.method2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mothod_MouseMove);
            // 
            // method1
            // 
            this.method1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.method1.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.method1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.method1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.method1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.method1.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.method1.ImageAlignment = FlatButton._ImgAlign.Left;
            this.method1.ImageIcon = null;
            this.method1.Location = new System.Drawing.Point(12, 15);
            this.method1.Name = "method1";
            this.method1.Rounded = false;
            this.method1.Size = new System.Drawing.Size(141, 24);
            this.method1.Style = FlatButton._Style.TextOnly;
            this.method1.TabIndex = 73;
            this.method1.Text = "Normal method";
            this.method1.Click += new System.EventHandler(this.Method_Click);
            this.method1.MouseLeave += new System.EventHandler(this.Mothod_MouseLeave);
            this.method1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mothod_MouseMove);
            // 
            // frmComplianceTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(164)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(439, 142);
            this.Controls.Add(this.DeviceContainer);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComplianceTest";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.Deactivate += new System.EventHandler(this.FrmDiscovery_Deactivate);
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
            this.DeviceContainer.ResumeLayout(false);
            this.aboutPanel.ResumeLayout(false);
            this.aboutPanel.PerformLayout();
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
        private System.ComponentModel.BackgroundWorker DiscoverWorker;
        private System.Windows.Forms.Panel DeviceContainer;
        private FlatButton method1;
        private FlatButton method2;
        private System.Windows.Forms.Panel aboutPanel;
        private System.Windows.Forms.Label about;
    }
}