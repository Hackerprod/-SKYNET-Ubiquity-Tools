﻿namespace DeviceView
{
    partial class frmDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDevice));
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.associd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aprepeater = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rssi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rx_chainmask = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tx_latency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uptime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.distance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.devices1 = new DeviceView.Devices();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 426);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1122, 24);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.devices1);
            this.panel1.Controls.Add(this._lvAliveHosts);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(1122, 426);
            this.panel1.TabIndex = 4;
            // 
            // _lvAliveHosts
            // 
            this._lvAliveHosts.BackColor = System.Drawing.Color.White;
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
            this.associd,
            this.aprepeater,
            this.rssi,
            this.rx_chainmask,
            this.idle,
            this.tx_latency,
            this.uptime,
            this.ack,
            this.distance});
            this._lvAliveHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lvAliveHosts.ForeColor = System.Drawing.Color.DimGray;
            this._lvAliveHosts.FullRowSelect = true;
            this._lvAliveHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvAliveHosts.Location = new System.Drawing.Point(8, 8);
            this._lvAliveHosts.Name = "_lvAliveHosts";
            this._lvAliveHosts.Size = new System.Drawing.Size(1106, 410);
            this._lvAliveHosts.TabIndex = 1;
            this._lvAliveHosts.UseCompatibleStateImageBehavior = false;
            this._lvAliveHosts.View = System.Windows.Forms.View.Details;
            this._lvAliveHosts.MouseClick += new System.Windows.Forms.MouseEventHandler(this._lvAliveHosts_MouseClick);
            this._lvAliveHosts.MouseHover += new System.EventHandler(this._lvAliveHosts_MouseHover);
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
            // MAC
            // 
            this.MAC.Text = "MAC";
            this.MAC.Width = 107;
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
            // txpower
            // 
            this.txpower.Text = "TX Power";
            // 
            // noisefloor
            // 
            this.noisefloor.Text = "Noise";
            this.noisefloor.Width = 39;
            // 
            // associd
            // 
            this.associd.Text = "Associd";
            this.associd.Width = 50;
            // 
            // aprepeater
            // 
            this.aprepeater.Text = "Aprepeater";
            this.aprepeater.Width = 66;
            // 
            // rssi
            // 
            this.rssi.Text = "RSSI";
            this.rssi.Width = 37;
            // 
            // rx_chainmask
            // 
            this.rx_chainmask.Text = "RX Chainmask";
            this.rx_chainmask.Width = 83;
            // 
            // idle
            // 
            this.idle.Text = "Idle";
            this.idle.Width = 30;
            // 
            // tx_latency
            // 
            this.tx_latency.Text = "TX Latency";
            this.tx_latency.Width = 67;
            // 
            // uptime
            // 
            this.uptime.Text = "Uptime";
            this.uptime.Width = 46;
            // 
            // ack
            // 
            this.ack.Text = "ACK";
            this.ack.Width = 35;
            // 
            // distance
            // 
            this.distance.Text = "Distance";
            this.distance.Width = 54;
            // 
            // devices1
            // 
            this.devices1.Average = null;
            this.devices1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.devices1.Location = new System.Drawing.Point(297, 172);
            this.devices1.Name = "devices1";
            this.devices1.Padding = new System.Windows.Forms.Padding(5);
            this.devices1.Size = new System.Drawing.Size(543, 183);
            this.devices1.TabIndex = 2;
            this.devices1.Visible = false;
            this.devices1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Devices1_MouseClick);
            this.devices1.MouseLeave += new System.EventHandler(this.Devices1_MouseLeave);
            this.devices1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Devices1_MouseMove);
            // 
            // frmDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1122, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FrmDevice_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView _lvAliveHosts;
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
        private System.Windows.Forms.ColumnHeader associd;
        private System.Windows.Forms.ColumnHeader aprepeater;
        private System.Windows.Forms.ColumnHeader rssi;
        private System.Windows.Forms.ColumnHeader rx_chainmask;
        private System.Windows.Forms.ColumnHeader idle;
        private System.Windows.Forms.ColumnHeader tx_latency;
        private System.Windows.Forms.ColumnHeader uptime;
        private System.Windows.Forms.ColumnHeader ack;
        private System.Windows.Forms.ColumnHeader distance;
        private Devices devices1;
    }
}

