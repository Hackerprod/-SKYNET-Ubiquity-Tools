using SKYNET.GUI.Controls;
using SKYNET.Properties;

namespace SKYNET.GUI
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
            this.PN_Top = new System.Windows.Forms.Panel();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DeviceContainer = new System.Windows.Forms.Panel();
            this.BT_USA = new SKYNET.GUI.Controls.SKYNET_Button();
            this.BT_International = new SKYNET.GUI.Controls.SKYNET_Button();
            this.PN_Top.SuspendLayout();
            this.panel3.SuspendLayout();
            this.DeviceContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PN_Top
            // 
            this.PN_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.PN_Top.Controls.Add(this.tittleLbl);
            this.PN_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.PN_Top.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.PN_Top.Location = new System.Drawing.Point(1, 1);
            this.PN_Top.Name = "PN_Top";
            this.PN_Top.Size = new System.Drawing.Size(213, 26);
            this.PN_Top.TabIndex = 5;
            // 
            // tittleLbl
            // 
            this.tittleLbl.AutoSize = true;
            this.tittleLbl.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tittleLbl.ForeColor = System.Drawing.Color.White;
            this.tittleLbl.Location = new System.Drawing.Point(5, 4);
            this.tittleLbl.Name = "tittleLbl";
            this.tittleLbl.Size = new System.Drawing.Size(123, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Define device type";
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
            this.panel3.Location = new System.Drawing.Point(1, 134);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(213, 24);
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
            // DeviceContainer
            // 
            this.DeviceContainer.AutoScroll = true;
            this.DeviceContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.DeviceContainer.Controls.Add(this.BT_USA);
            this.DeviceContainer.Controls.Add(this.BT_International);
            this.DeviceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceContainer.Location = new System.Drawing.Point(1, 27);
            this.DeviceContainer.Name = "DeviceContainer";
            this.DeviceContainer.Size = new System.Drawing.Size(213, 107);
            this.DeviceContainer.TabIndex = 67;
            // 
            // BT_USA
            // 
            this.BT_USA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_USA.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.BT_USA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_USA.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BT_USA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.BT_USA.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.BT_USA.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.BT_USA.ImageIcon = null;
            this.BT_USA.Location = new System.Drawing.Point(37, 63);
            this.BT_USA.MenuMode = false;
            this.BT_USA.Name = "BT_USA";
            this.BT_USA.Rounded = false;
            this.BT_USA.Size = new System.Drawing.Size(141, 31);
            this.BT_USA.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.BT_USA.TabIndex = 74;
            this.BT_USA.Text = "USA Version";
            this.BT_USA.Click += new System.EventHandler(this.Method_Click);
            // 
            // BT_International
            // 
            this.BT_International.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.BT_International.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.BT_International.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_International.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BT_International.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this.BT_International.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.BT_International.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.BT_International.ImageIcon = null;
            this.BT_International.Location = new System.Drawing.Point(37, 19);
            this.BT_International.MenuMode = false;
            this.BT_International.Name = "BT_International";
            this.BT_International.Rounded = false;
            this.BT_International.Size = new System.Drawing.Size(141, 31);
            this.BT_International.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.BT_International.TabIndex = 73;
            this.BT_International.Text = "International Version";
            this.BT_International.Click += new System.EventHandler(this.Method_Click);
            // 
            // frmComplianceTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(164)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(215, 159);
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
            this.Name = "frmComplianceTest";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.PN_Top.ResumeLayout(false);
            this.PN_Top.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.DeviceContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PN_Top;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.WebBrowser Browser;
        public System.Windows.Forms.Label tittleLbl;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel DeviceContainer;
        private SKYNET_Button BT_International;
        private SKYNET_Button BT_USA;
    }
}