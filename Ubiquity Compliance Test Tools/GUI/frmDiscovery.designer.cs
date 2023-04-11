

using SKYNET.GUI.Controls;
using SKYNET.Properties;

namespace SKYNET.GUI
{
    partial class frmDiscovery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDiscovery));
            this.PN_Top = new System.Windows.Forms.Panel();
            this.BT_Close = new SKYNET.Controls.SKYNET_Box();
            this.tittleLbl = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DiscoverWorker = new System.ComponentModel.BackgroundWorker();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.Recargar = new SKYNET.GUI.Controls.SKYNET_Button();
            this.DeviceContainer = new System.Windows.Forms.Panel();
            this.usarParaConectarPorSSHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirEquipoDesdeLaWebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarDatoDeEsteEquipoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BoxMenu = new SKYNET.GUI.Controls.SKYNET_ContextMenuStrip();
            this.PN_Top.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelSeparator.SuspendLayout();
            this.BoxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PN_Top
            // 
            this.PN_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.PN_Top.Controls.Add(this.BT_Close);
            this.PN_Top.Controls.Add(this.tittleLbl);
            this.PN_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.PN_Top.ForeColor = System.Drawing.Color.White;
            this.PN_Top.Location = new System.Drawing.Point(0, 0);
            this.PN_Top.Name = "PN_Top";
            this.PN_Top.Size = new System.Drawing.Size(360, 26);
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
            this.BT_Close.Location = new System.Drawing.Point(326, 0);
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
            this.tittleLbl.Size = new System.Drawing.Size(193, 16);
            this.tittleLbl.TabIndex = 7;
            this.tittleLbl.Text = "Detection of device on the network";
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
            this.panel3.Location = new System.Drawing.Point(0, 480);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 24);
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
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.panelSeparator.Controls.Add(this.Recargar);
            this.panelSeparator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSeparator.Location = new System.Drawing.Point(0, 443);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(360, 37);
            this.panelSeparator.TabIndex = 66;
            // 
            // Recargar
            // 
            this.Recargar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.Recargar.BackColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.Recargar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Recargar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Recargar.ForeColor = System.Drawing.Color.White;
            this.Recargar.ForeColorMouseOver = System.Drawing.Color.Empty;
            this.Recargar.ImageAlignment = SKYNET.GUI.Controls.SKYNET_Button._ImgAlign.Left;
            this.Recargar.ImageIcon = null;
            this.Recargar.Location = new System.Drawing.Point(4, 7);
            this.Recargar.MenuMode = false;
            this.Recargar.Name = "Recargar";
            this.Recargar.Rounded = false;
            this.Recargar.Size = new System.Drawing.Size(141, 24);
            this.Recargar.Style = SKYNET.GUI.Controls.SKYNET_Button._Style.TextOnly;
            this.Recargar.TabIndex = 72;
            this.Recargar.Text = "Detectar dispositivos";
            this.Recargar.Click += new System.EventHandler(this.Recargar_Click);
            // 
            // DeviceContainer
            // 
            this.DeviceContainer.AutoScroll = true;
            this.DeviceContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.DeviceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceContainer.Location = new System.Drawing.Point(0, 26);
            this.DeviceContainer.Name = "DeviceContainer";
            this.DeviceContainer.Size = new System.Drawing.Size(360, 417);
            this.DeviceContainer.TabIndex = 67;
            // 
            // usarParaConectarPorSSHToolStripMenuItem
            // 
            this.usarParaConectarPorSSHToolStripMenuItem.Name = "usarParaConectarPorSSHToolStripMenuItem";
            this.usarParaConectarPorSSHToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.usarParaConectarPorSSHToolStripMenuItem.Text = "Usar para conectar por SSH";
            this.usarParaConectarPorSSHToolStripMenuItem.Click += new System.EventHandler(this.UsarParaConectarPorSSHToolStripMenuItem_Click);
            // 
            // abrirEquipoDesdeLaWebToolStripMenuItem
            // 
            this.abrirEquipoDesdeLaWebToolStripMenuItem.Name = "abrirEquipoDesdeLaWebToolStripMenuItem";
            this.abrirEquipoDesdeLaWebToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.abrirEquipoDesdeLaWebToolStripMenuItem.Text = "Abrir equipo desde la web";
            this.abrirEquipoDesdeLaWebToolStripMenuItem.Click += new System.EventHandler(this.AbrirEquipoDesdeLaWebToolStripMenuItem_Click);
            // 
            // guardarDatoDeEsteEquipoToolStripMenuItem
            // 
            this.guardarDatoDeEsteEquipoToolStripMenuItem.Name = "guardarDatoDeEsteEquipoToolStripMenuItem";
            this.guardarDatoDeEsteEquipoToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.guardarDatoDeEsteEquipoToolStripMenuItem.Text = "Guardar datos de este equipo";
            this.guardarDatoDeEsteEquipoToolStripMenuItem.Click += new System.EventHandler(this.GuardarDatoDeEsteEquipoToolStripMenuItem_Click);
            // 
            // guardarDatosDeTodosLosEquiposToolStripMenuItem
            // 
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem.Name = "guardarDatosDeTodosLosEquiposToolStripMenuItem";
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem.Text = "Guardar datos de todos los equipos";
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem.Click += new System.EventHandler(this.GuardarDatosDeTodosLosEquiposToolStripMenuItem_Click);
            // 
            // BoxMenu
            // 
            this.BoxMenu.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BoxMenu.ForeColor = System.Drawing.Color.White;
            this.BoxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usarParaConectarPorSSHToolStripMenuItem,
            this.abrirEquipoDesdeLaWebToolStripMenuItem,
            this.guardarDatoDeEsteEquipoToolStripMenuItem,
            this.guardarDatosDeTodosLosEquiposToolStripMenuItem});
            this.BoxMenu.Name = "flatContextMenuStrip1";
            this.BoxMenu.ShowImageMargin = false;
            this.BoxMenu.Size = new System.Drawing.Size(236, 92);
            // 
            // frmDiscovery
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(31)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(360, 504);
            this.Controls.Add(this.DeviceContainer);
            this.Controls.Add(this.panelSeparator);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.PN_Top);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(1440, 860);
            this.Name = "frmDiscovery";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubiquity Compliance Test Tool";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.PN_Top.ResumeLayout(false);
            this.PN_Top.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelSeparator.ResumeLayout(false);
            this.BoxMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Panel DeviceContainer;
        private SKYNET_Button Recargar;
        private System.Windows.Forms.ToolStripMenuItem usarParaConectarPorSSHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirEquipoDesdeLaWebToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarDatoDeEsteEquipoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarDatosDeTodosLosEquiposToolStripMenuItem;
        public SKYNET_ContextMenuStrip BoxMenu;
        private SKYNET.Controls.SKYNET_Box BT_Close;
    }
}