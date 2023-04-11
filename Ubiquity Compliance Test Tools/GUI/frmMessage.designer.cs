using SKYNET.GUI.Controls;

namespace SKYNET.GUI
{
    partial class frmMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessage));
            this.acceptBtn = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.acepctBtn = new SKYNET_Button();
            this.cancelBtn = new SKYNET_Button();
            this.panel15 = new System.Windows.Forms.Panel();
            this.Cancel = new System.Windows.Forms.Button();
            this.txtMessage = new SKYNET_TextBox();
            this.PN_Top = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel15.SuspendLayout();
            this.PN_Top.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptBtn
            // 
            this.acceptBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptBtn.Location = new System.Drawing.Point(485, 375);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 16;
            this.acceptBtn.Text = "button1";
            this.acceptBtn.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(483, 145);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(18, 23);
            this.ok.TabIndex = 24;
            this.ok.Text = "ok";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(32)))));
            this.panel1.Controls.Add(this.acepctBtn);
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Controls.Add(this.panel15);
            this.panel1.Controls.Add(this.PN_Top);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 193);
            this.panel1.TabIndex = 25;
            // 
            // acepctBtn
            // 
            this.acepctBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
            this.acepctBtn.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.acepctBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.acepctBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.acepctBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.acepctBtn.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.acepctBtn.ImageAlignment = SKYNET_Button._ImgAlign.Left;
            this.acepctBtn.ImageIcon = null;
            this.acepctBtn.Location = new System.Drawing.Point(273, 152);
            this.acepctBtn.Name = "acepctBtn";
            this.acepctBtn.Rounded = false;
            this.acepctBtn.Size = new System.Drawing.Size(93, 29);
            this.acepctBtn.Style = SKYNET_Button._Style.TextOnly;
            this.acepctBtn.TabIndex = 27;
            this.acepctBtn.Text = "OK";
            this.acepctBtn.Click += new System.EventHandler(this.acepctBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
            this.cancelBtn.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.cancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cancelBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cancelBtn.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.cancelBtn.ImageAlignment = SKYNET_Button._ImgAlign.Left;
            this.cancelBtn.ImageIcon = null;
            this.cancelBtn.Location = new System.Drawing.Point(372, 152);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Rounded = false;
            this.cancelBtn.Size = new System.Drawing.Size(93, 29);
            this.cancelBtn.Style = SKYNET_Button._Style.TextOnly;
            this.cancelBtn.TabIndex = 26;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(32)))));
            this.panel15.Controls.Add(this.Cancel);
            this.panel15.Controls.Add(this.txtMessage);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.panel15.Location = new System.Drawing.Point(0, 26);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(475, 120);
            this.panel15.TabIndex = 25;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(484, 90);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(16, 23);
            this.Cancel.TabIndex = 25;
            this.Cancel.Text = "cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(29)))), ((int)(((byte)(32)))));
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtMessage.Location = new System.Drawing.Point(12, 10);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(453, 105);
            this.txtMessage.TabIndex = 27;
            // 
            // PN_Top
            // 
            this.PN_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
            this.PN_Top.Controls.Add(this.label1);
            this.PN_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.PN_Top.ForeColor = System.Drawing.Color.White;
            this.PN_Top.Location = new System.Drawing.Point(0, 0);
            this.PN_Top.Name = "PN_Top";
            this.PN_Top.Size = new System.Drawing.Size(475, 26);
            this.PN_Top.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(157)))), ((int)(((byte)(160)))));
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 0;
            // 
            // frmMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(164)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(477, 195);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.acceptBtn);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1440, 860);
            this.Name = "frmMessage";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message";
            this.panel1.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.PN_Top.ResumeLayout(false);
            this.PN_Top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Panel panel1;
        private SKYNET_Button acepctBtn;
        private SKYNET_Button cancelBtn;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Button Cancel;
        private SKYNET_TextBox txtMessage;
        private System.Windows.Forms.Panel PN_Top;
        private System.Windows.Forms.Label label1;
    }
}