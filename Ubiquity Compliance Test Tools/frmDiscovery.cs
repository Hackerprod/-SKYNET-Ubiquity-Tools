using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using SKYNET.LOG;
using Renci.SshNet;
using SkynetChat.Controles;
using UbntDiscovery;

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmDiscovery : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmDiscovery frm;
        private static ILog ilog_0;
        public StringBuilder HtmlString;
        public bool Searching;
        private SshCommand sshCommand;
        private int deviceDiscovereds;
        private int y = 7;
        internal BoxTool boxTool;

        public DeviceDiscovery DeviceDiscovery { get; private set; }

        private CancellationTokenSource _cts;
        private Task discoveryTask;

        public frmDiscovery()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;

            ilog_0 = new ILog();
            //deviceDiscovereds = new List<DeviceDiscovered>();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            label1.Text = "Buscando dispositivos en la red";

            DeviceDiscovery = new DeviceDiscovery();
            DeviceDiscovery.DeviceDiscovered += DeviceDiscovery_DeviceDiscovered;

            _ = ScanAsync();
        }

        private void DeviceDiscovery_DeviceDiscovered(UbntDiscovery.Device device)
        {
            deviceDiscovereds = deviceDiscovereds + 1;
            DeviceContainer.Controls.Add(new BoxTool()
            {
                Device = device.LongPlatform,
                BoxName = device.Hostname,
                IpName = device.FirstAddress.IpAddress.ToString(),
                MAC = device.FormatedMacAddress,

                Modo = device.WirelessModeDescription,
                SSID = device.SSID,
                Firmware = modCommon.Firmware(device.Firmware),
                Uptime = device.Uptime.ToString("d\\d\\ hh\\:mm\\:ss"),
                //StringFormat = '{}{0:d\\d\\ hh\\:mm\\:ss}'}

                Location = new System.Drawing.Point(12, y),
                Margin = new System.Windows.Forms.Padding(3, 4, 3, 4),
                Size = new System.Drawing.Size(332, 55),
            });
            y = y + 62;

            label1.Text = deviceDiscovereds + " dispositivos encontrados";

        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Control control = (Control)sender;
                if (control is PictureBox)
                {
                    switch (control.Name)
                    {
                        case "ClosePic": CloseBox.BackColor = Color.FromArgb(53, 64, 78); break;
                    }
                }
                if (control is Panel)
                {
                    switch (control.Name)
                    {
                        case "CloseBox": CloseBox.BackColor = Color.FromArgb(53, 64, 78); break;
                    }
                }
            }   catch { }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            CloseBox.BackColor = Color.FromArgb(43, 54, 68);
        }

        private void Event_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point((Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);
                Update();
                Opacity = 0.93;
            }
        }

        private void Event_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;

        }

        private void Event_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            Opacity = 100;
        }

        private void TittleLbl_Click(object sender, EventArgs e)
        {

        }

        private void DiscoverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (DeviceDiscovery.IsScanning)
                {
                    Recargar.Enabled = false;
                }
                else
                {
                    Recargar.Enabled = true;
                }
            }

        }


        private void Recargar_Click(object sender, EventArgs e)
        {
            label1.Text = "Buscando dispositivos en la red";
            
            deviceDiscovereds = 0;
            y = 7;

            _ = ScanAsync();
        }

        public async Task ScanAsync()
        {
            if (discoveryTask != null && (discoveryTask.Status == TaskStatus.WaitingForActivation || discoveryTask.Status == TaskStatus.Running))
            {
                _cts.Cancel();
                DeviceContainer.Controls.Clear();
            }

            try
            {
                var cts = new CancellationTokenSource();
                _cts = cts;
                discoveryTask = DeviceDiscovery.DiscoveryAsync(_cts.Token).ContinueWith((t) => cts.Dispose());

                await discoveryTask;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FrmDiscovery_Deactivate(object sender, EventArgs e)
        {
            //Close();
        }

        private void UsarParaConectarPorSSHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMain.frm.SetEquipo(boxTool.IpName);
            Close();
        }

        private void AbrirEquipoDesdeLaWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://" + boxTool.IpName);
            }
            catch { }
        }

        private void GuardarDatoDeEsteEquipoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result =
                "Device Type:      " + boxTool.Device + Environment.NewLine +
                "Device Name:      " + boxTool.BoxName + Environment.NewLine +
                "Device MAC:       " + boxTool.MAC + Environment.NewLine +
                "Device IP:        " + boxTool.IpName + Environment.NewLine +
                "Device Mode:      " + boxTool.Modo + Environment.NewLine +
                "Device SSID:      " + boxTool.SSID + Environment.NewLine +
                "Firmware version: " + boxTool.Firmware + Environment.NewLine +
                "Device uptime:    " + boxTool.Uptime;
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "All files (*.*)|*.*",
                FileName = boxTool.BoxName + ".txt",
            };
            DialogResult Dresult = dialog.ShowDialog();
            if (Dresult == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, result);
            }
        }



        private void GuardarDatosDeTodosLosEquiposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result = "Devices in the Network" + Environment.NewLine + "/////////////////////////////////////////////////////////////////";

            for (int i = 0; i < DeviceContainer.Controls.Count; i++)
            {
                if (DeviceContainer.Controls[i] is BoxTool)
                {
                    BoxTool tool = (BoxTool)DeviceContainer.Controls[i];
                    result += Environment.NewLine + Environment.NewLine +
                    "Device Type:      " + tool.Device + Environment.NewLine +
                    "Device Name:      " + tool.BoxName + Environment.NewLine +
                    "Device MAC:       " + tool.MAC + Environment.NewLine +
                    "Device IP:        " + tool.IpName + Environment.NewLine +
                    "Device Mode:      " + tool.Modo + Environment.NewLine +
                    "Device SSID:      " + tool.SSID + Environment.NewLine +
                    "Firmware version: " + tool.Firmware + Environment.NewLine +
                    "Device uptime:    " + tool.Uptime + Environment.NewLine + Environment.NewLine + 
                    "/////////////////////////////////////////////////////////////////";
                }
            }
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "All files (*.*)|*.*",
                FileName = "Devices" + ".txt",
            };
            DialogResult Dresult = dialog.ShowDialog();
            if (Dresult == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, result);
            }

        }

        private void SaveFile(BoxTool boxTool)
        {

        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            int attrValue = 2;
            DwmApi.DwmSetWindowAttribute(base.Handle, attrValue, ref attrValue, 8);
            DwmApi.MARGINS mARGINS = new DwmApi.MARGINS();
            mARGINS.cyBottomHeight = 1;
            mARGINS.cxLeftWidth = 0;
            mARGINS.cxRightWidth = 0;
            mARGINS.cyTopHeight = 0;
            DwmApi.MARGINS marInset = mARGINS;
            DwmApi.DwmExtendFrameIntoClientArea(base.Handle, ref marInset);

        }

    }
}
