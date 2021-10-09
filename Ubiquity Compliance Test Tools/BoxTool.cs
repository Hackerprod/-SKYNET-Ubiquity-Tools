using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using SKYNET;

namespace SkynetChat.Controles
{
    public class BoxTool : UserControl
    {
        private IContainer components;
        private PictureBox Icon;
        private Label name;
        private Label ip;
        private Label device;
        private Label Mac;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label firmware;
        private Label modo;
        private Label uptime;
        private Label ssid;
        public string HostName;

        internal enum ConnectionStatus
        {
            Online,
            Offline
        }


        public BoxTool()
        {
            this.InitializeComponent();
        }
        private string _BoxName { get; set; }
        public string BoxName
        {
            get
            {
                return _BoxName;
            }
            set
            {
                _BoxName = value;
                name.Text = value;
            }
        }

        private string _MAC { get; set; }
        public string MAC
        {
            get
            {
                return _MAC;
            }
            set
            {
                _MAC = value;
                Mac.Text = value;
            }
        }
        private string _Device { get; set; }
        public string Device
        {
            get
            {
                return _Device;
            }
            set
            {
                _Device = value;
                device.Text = value;
                Icon.Image = modCommon.GetDeviceImage(value);
            }
        }

        private string _IpName { get; set; }
        public string IpName
        {
            get
            {
                return _IpName;
            }
            set
            {
                _IpName = value;
                ip.Text = value;
            }
        }

        private string _Modo { get; set; }
        public string Modo
        {
            get
            {
                return _Modo;
            }
            set
            {
                _Modo = value;
                modo.Text = value;
            }
        }
        private string _SSID { get; set; }
        public string SSID
        {
            get
            {
                return _SSID;
            }
            set
            {
                _SSID = value;
                ssid.Text = value;
            }
        }
        private string _Firmware { get; set; }
        public string Firmware
        {
            get
            {
                return _Firmware;
            }
            set
            {
                _Firmware = value;
                firmware.Text = value;
            }
        }
        private string _Uptime { get; set; }
        public string Uptime
        {
            get
            {
                return _Uptime;
            }
            set
            {
                _Uptime = value;
                uptime.Text = value;
            }
        }

        











        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing || this.components == null)
                    return;
                this.components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.name = new System.Windows.Forms.Label();
            this.ip = new System.Windows.Forms.Label();
            this.Icon = new System.Windows.Forms.PictureBox();
            this.device = new System.Windows.Forms.Label();
            this.Mac = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.firmware = new System.Windows.Forms.Label();
            this.modo = new System.Windows.Forms.Label();
            this.uptime = new System.Windows.Forms.Label();
            this.ssid = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.name.Location = new System.Drawing.Point(91, 14);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(34, 12);
            this.name.TabIndex = 1;
            this.name.Text = "Name";
            this.name.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.name.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.name.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.name.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // ip
            // 
            this.ip.AutoSize = true;
            this.ip.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ip.Location = new System.Drawing.Point(91, 37);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(41, 12);
            this.ip.TabIndex = 2;
            this.ip.Text = "127.0.0.1";
            this.ip.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.ip.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.ip.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.ip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // Icon
            // 
            this.Icon.Image = global::SKYNET.Properties.Resources.loco_m2;
            this.Icon.Location = new System.Drawing.Point(4, 3);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(39, 49);
            this.Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Icon.TabIndex = 0;
            this.Icon.TabStop = false;
            this.Icon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.Icon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.Icon.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.Icon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // device
            // 
            this.device.AutoSize = true;
            this.device.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.device.Location = new System.Drawing.Point(91, 2);
            this.device.Name = "device";
            this.device.Size = new System.Drawing.Size(37, 12);
            this.device.TabIndex = 3;
            this.device.Text = "Device";
            this.device.DoubleClick += new System.EventHandler(this.Box_MouseClick);
            this.device.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.device.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.device.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.device.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // Mac
            // 
            this.Mac.AutoSize = true;
            this.Mac.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mac.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Mac.Location = new System.Drawing.Point(91, 26);
            this.Mac.Name = "Mac";
            this.Mac.Size = new System.Drawing.Size(25, 12);
            this.Mac.TabIndex = 4;
            this.Mac.Text = "MAC";
            this.Mac.DoubleClick += new System.EventHandler(this.Box_MouseClick);
            this.Mac.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.Mac.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.Mac.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.Mac.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(49, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Device:";
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.label1.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Location = new System.Drawing.Point(49, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name:";
            this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.label2.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Location = new System.Drawing.Point(49, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "MAC:";
            this.label3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.label3.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Location = new System.Drawing.Point(49, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "IP:";
            this.label4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label4.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.label4.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label5.Location = new System.Drawing.Point(204, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "Uptime:";
            this.label5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label5.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label6.Location = new System.Drawing.Point(204, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "Firmware:";
            this.label6.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label6.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label7.Location = new System.Drawing.Point(204, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "SSID:";
            this.label7.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label7.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label8.Location = new System.Drawing.Point(204, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "Modo:";
            this.label8.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.label8.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.label8.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // firmware
            // 
            this.firmware.AutoSize = true;
            this.firmware.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firmware.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.firmware.Location = new System.Drawing.Point(259, 26);
            this.firmware.Name = "firmware";
            this.firmware.Size = new System.Drawing.Size(44, 12);
            this.firmware.TabIndex = 12;
            this.firmware.Text = "Firmware:";
            this.firmware.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.firmware.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.firmware.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // modo
            // 
            this.modo.AutoSize = true;
            this.modo.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.modo.Location = new System.Drawing.Point(259, 2);
            this.modo.Name = "modo";
            this.modo.Size = new System.Drawing.Size(32, 12);
            this.modo.TabIndex = 11;
            this.modo.Text = "modo";
            this.modo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.modo.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.modo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // uptime
            // 
            this.uptime.AutoSize = true;
            this.uptime.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uptime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.uptime.Location = new System.Drawing.Point(259, 37);
            this.uptime.Name = "uptime";
            this.uptime.Size = new System.Drawing.Size(34, 12);
            this.uptime.TabIndex = 10;
            this.uptime.Text = "Uptime";
            this.uptime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.uptime.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.uptime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // ssid
            // 
            this.ssid.AutoSize = true;
            this.ssid.Font = new System.Drawing.Font("Segoe UI Symbol", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ssid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssid.Location = new System.Drawing.Point(259, 14);
            this.ssid.Name = "ssid";
            this.ssid.Size = new System.Drawing.Size(27, 12);
            this.ssid.TabIndex = 9;
            this.ssid.Text = "SSID";
            this.ssid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.ssid.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.ssid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            // 
            // BoxTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.firmware);
            this.Controls.Add(this.modo);
            this.Controls.Add(this.uptime);
            this.Controls.Add(this.ssid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Mac);
            this.Controls.Add(this.device);
            this.Controls.Add(this.ip);
            this.Controls.Add(this.name);
            this.Controls.Add(this.Icon);
            this.Name = "BoxTool";
            this.Size = new System.Drawing.Size(370, 55);
            this.Load += new System.EventHandler(this.BoxTool_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Box_MouseDoubleClick);
            this.MouseLeave += new System.EventHandler(this.Box_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Box_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Box_MouseLeave(object sender, EventArgs e)
        {
            name.ForeColor = Color.FromArgb(147, 157, 160);
            ip.ForeColor = Color.FromArgb(147, 157, 160);
            Mac.ForeColor = Color.FromArgb(147, 157, 160);
            device.ForeColor = Color.FromArgb(147, 157, 160);

            this.BackColor = Color.FromArgb(43, 54, 68);

            modo.ForeColor = Color.FromArgb(147, 157, 160);
            ssid.ForeColor = Color.FromArgb(147, 157, 160);
            firmware.ForeColor = Color.FromArgb(147, 157, 160);
            uptime.ForeColor = Color.FromArgb(147, 157, 160);

        }

        private void Box_MouseMove(object sender, MouseEventArgs e)
        {
            name.ForeColor = Color.FromArgb(255, 255, 255);
            ip.ForeColor = Color.FromArgb(255, 255, 255);
            Mac.ForeColor = Color.FromArgb(255, 255, 255);
            device.ForeColor = Color.FromArgb(255, 255, 255);
            this.BackColor = Color.FromArgb(53, 64, 78);

            modo.ForeColor = Color.FromArgb(255, 255, 255);
            ssid.ForeColor = Color.FromArgb(255, 255, 255);
            firmware.ForeColor = Color.FromArgb(255, 255, 255);
            uptime.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Process.Start("https://" + IpName);
                }
            }
            catch { }

        }
        private void Box_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ShowMenuInBox(this, e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    frmMain.frm.SetEquipo(IpName);
                }
            }
            catch { }
        }

        private void ShowMenuInBox(BoxTool box, int xx, int yy)
        {
            Rectangle cellDisplayRectangle = box.DisplayRectangle;
            frmDiscovery.frm.boxTool = box;
            frmDiscovery.frm.BoxMenu.Show(box, cellDisplayRectangle.Left + xx, cellDisplayRectangle.Top + yy);
        }

        private void Box_MouseClick(object sender, EventArgs e)
        {

        }

        private void BoxTool_Load(object sender, EventArgs e)
        {

        }
    }
}
