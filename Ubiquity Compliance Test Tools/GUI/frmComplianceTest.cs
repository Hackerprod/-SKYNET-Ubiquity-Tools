using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mshtml;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;
using SKYNET.Server;
using SKYNET.LOG;
using System.Timers;
using Microsoft.VisualBasic;
using Renci.SshNet;
using System.Net.NetworkInformation;
using SKYNET.Properties;
using SkynetChat.Controles;
using UbntDiscovery;

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmComplianceTest : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmComplianceTest frm;
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

        public frmComplianceTest()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;

            ilog_0 = new ILog();
            //deviceDiscovereds = new List<DeviceDiscovered>();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

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
                        case "ClosePic": CloseBox.BackColor = Color.FromArgb(53, 54, 58); break;
                    }
                }
                if (control is Panel)
                {
                    switch (control.Name)
                    {
                        case "CloseBox": CloseBox.BackColor = Color.FromArgb(53, 54, 58); break;
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
                    method1.Enabled = false;
                }
                else
                {
                    method1.Enabled = true;
                }
            }

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

        private void Mothod_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FlatButton)
            {
                FlatButton button = (FlatButton)sender;
                if (button.Name == "method1")
                {
                    about.Text = "Basic method. No reboot required";
                }
                else if (button.Name == "method2")
                {
                    about.Text = "Advanced method. Requires reboot";
                }

                aboutPanel.Location = new Point(aboutPanel.Location.X, button.Location.Y);
                aboutPanel.Visible = true;

            }
        }

        private void Mothod_MouseLeave(object sender, EventArgs e)
        {
            aboutPanel.Visible = false;
        }

        private void Method_Click(object sender, EventArgs e)
        {
            if (sender is FlatButton)
            {
                FlatButton button = (FlatButton)sender;
                if (button.Name == "method1")
                {
                    Common.Metodo = Common.Method.Method_1;
                }
                else if (button.Name == "method2")
                {
                    Common.Metodo = Common.Method.Method_2;
                }
                Close();
            }
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
