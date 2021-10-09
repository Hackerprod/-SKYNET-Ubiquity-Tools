namespace DeviceView
{
    partial class Devices
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.Info = new System.Windows.Forms.Label();
            this._lvAliveHosts = new System.Windows.Forms.ListView();
            this.Chann = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.signal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ccq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.noisefloor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tx_latency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Info);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 158);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 20);
            this.panel1.TabIndex = 3;
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Location = new System.Drawing.Point(3, 4);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(53, 13);
            this.Info.TabIndex = 0;
            this.Info.Text = "Average: ";
            this.Info.Click += new System.EventHandler(this.Info_Click);
            // 
            // _lvAliveHosts
            // 
            this._lvAliveHosts.BackColor = System.Drawing.Color.White;
            this._lvAliveHosts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._lvAliveHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Chann,
            this.DeviceName,
            this.signal,
            this.lastip,
            this.tx,
            this.rx,
            this.ccq,
            this.noisefloor,
            this.tx_latency});
            this._lvAliveHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lvAliveHosts.ForeColor = System.Drawing.Color.DimGray;
            this._lvAliveHosts.FullRowSelect = true;
            this._lvAliveHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvAliveHosts.Location = new System.Drawing.Point(5, 5);
            this._lvAliveHosts.Name = "_lvAliveHosts";
            this._lvAliveHosts.Size = new System.Drawing.Size(533, 153);
            this._lvAliveHosts.TabIndex = 4;
            this._lvAliveHosts.UseCompatibleStateImageBehavior = false;
            this._lvAliveHosts.View = System.Windows.Forms.View.Details;
            this._lvAliveHosts.MouseLeave += new System.EventHandler(this._lvAliveHosts_MouseLeave);
            this._lvAliveHosts.MouseMove += new System.Windows.Forms.MouseEventHandler(this._lvAliveHosts_MouseMove);
            // 
            // Chann
            // 
            this.Chann.Text = "Channel";
            this.Chann.Width = 52;
            // 
            // DeviceName
            // 
            this.DeviceName.Text = "Device Name";
            this.DeviceName.Width = 118;
            // 
            // signal
            // 
            this.signal.Text = "Signal";
            this.signal.Width = 42;
            // 
            // lastip
            // 
            this.lastip.Text = "IP Direction";
            this.lastip.Width = 101;
            // 
            // tx
            // 
            this.tx.Text = "TX";
            this.tx.Width = 33;
            // 
            // rx
            // 
            this.rx.Text = "RX";
            this.rx.Width = 33;
            // 
            // ccq
            // 
            this.ccq.Text = "CCQ";
            this.ccq.Width = 35;
            // 
            // noisefloor
            // 
            this.noisefloor.Text = "Noise";
            this.noisefloor.Width = 39;
            // 
            // tx_latency
            // 
            this.tx_latency.Text = "TX Latency";
            this.tx_latency.Width = 67;
            // 
            // Devices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this._lvAliveHosts);
            this.Controls.Add(this.panel1);
            this.Name = "Devices";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(543, 183);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Info;
        private System.Windows.Forms.ListView _lvAliveHosts;
        private System.Windows.Forms.ColumnHeader Chann;
        private System.Windows.Forms.ColumnHeader DeviceName;
        private System.Windows.Forms.ColumnHeader signal;
        private System.Windows.Forms.ColumnHeader lastip;
        private System.Windows.Forms.ColumnHeader tx;
        private System.Windows.Forms.ColumnHeader rx;
        private System.Windows.Forms.ColumnHeader ccq;
        private System.Windows.Forms.ColumnHeader noisefloor;
        private System.Windows.Forms.ColumnHeader tx_latency;
    }
}
