using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mshtml;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel;
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

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmManager : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmManager frm;
        private static ILog ilog_0;
        public StringBuilder HtmlString;
        public bool Searching;
        private bool connected;

        public bool Connected
        {
            get { return connected; }
            set
            {
                if (value == true)
                {
                    this.StatusLabel.Text = "ONLINE";
                    this.StatusLabel.ForeColor = Color.Green;

                    if (!pingWorker.IsBusy)
                        pingWorker.RunWorkerAsync();

                    modCommon.SetVisibleControl(PingLabel, true);
                    modCommon.SetVisibleControl(lblping, true);

                }
                else 
                {
                    this.StatusLabel.Text = "OFFLINE";
                    this.StatusLabel.ForeColor = Color.Red;

                    DeviceLabel.Text = "Offline";
                    FirmwareLabel.Text = "Offline";
                    CountryLabel.Text = "Offline";
                    StatusLabel.Text = "Offline";

                    modCommon.SetVisibleControl(PingLabel, false);
                    modCommon.SetVisibleControl(lblping, false);

                    pingWorker.CancelAsync();
                }
                connected = value;
            }
        }

        public frmManager()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;

            ilog_0 = new ILog();
            frmMain.frm.Visible = false;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            Connected = false;

            Initialize();
        }

        private void Initialize()
        {
            DeviceLabel.Text = frmMain.frm.DeviceLabel.Text;
            FirmwareLabel.Text = frmMain.frm.FirmwareLabel.Text;
            CountryLabel.Text = frmMain.frm.CountryLabel.Text;
            StatusLabel.Text = frmMain.frm.StatusLabel.Text;
            username.Text = frmMain.frm.username.Text;

            //Obtener lista de canales
            for (int i = 0; i < modCommon.device.ChannelList.Count; i++)
            {
                ChannelList.Items.Add(modCommon.device.ChannelList[i]);
            }
            if (modCommon.device.Channel != "")
            {
                if (modCommon.device.Channel.Length == 1) modCommon.device.Channel = "0" + modCommon.device.Channel;

                for (int i = 0; i < ChannelList.Items.Count; i++)
                {
                    if (ChannelList.Items[i].ToString().Contains(" " + modCommon.device.Channel + " "))
                    {
                        ChannelList.SelectedIndex = i;
                    }
                }
            }

            //Maxima potencia
            int Maxpower = Convert.ToInt32(modCommon.device.MaxPower);
            PowerBar.Maximum = Maxpower;

            //Potencia actual
            int currentpower = Convert.ToInt32(modCommon.device.Power);
            PowerBar.Value = currentpower;
            CurrentPower.Text = modCommon.device.Power;

            Connected = true;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;
        public static void ScrollToBottom(RichTextBox MyRichTextBox)
        {
            SendMessage(MyRichTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            frmMain.frm.Visible = true;
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
                        case "MinPic": MinBox.BackColor = Color.FromArgb(53, 64, 78); break;
                    }
                }
                if (control is Panel)
                {
                    switch (control.Name)
                    {
                        case "CloseBox": CloseBox.BackColor = Color.FromArgb(53, 64, 78); break;
                        case "MinBox": MinBox.BackColor = Color.FromArgb(53, 64, 78); break;
                    }
                }
            }   catch { }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            MinBox.BackColor = Color.FromArgb(43, 54, 68);
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

        private void Minimize_click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        internal void Write(object mess, MessageType mtype = MessageType.INFO)
        {
            rtbLogs.SelectionStart = rtbLogs.TextLength;
            rtbLogs.SelectionLength = 0;

            string time = " " + DateTime.Now.ToShortTimeString() + " ";

            WriteLine(mtype, time, mtype.ToString(), " " + mess.ToString());

        }
        private void WriteLine(MessageType type, string text1, string resaltar = "", string text2 = "")
        {
            Color color = Color.FromArgb(147, 157, 160);

            switch (type)
            {
                case MessageType.INFO:  color = ColorTranslator.FromHtml("#a8aab1"); resaltar = "INFO    "; break;
                case MessageType.WARN:  color = ColorTranslator.FromHtml("#f58207"); resaltar = "WARN  "; break;
                case MessageType.ERROR: color = ColorTranslator.FromHtml("#f50729"); resaltar = "ERROR "; break;
                case MessageType.DEBUG: color = ColorTranslator.FromHtml("#07a4f5"); resaltar = "DEBUG "; break;
            }
            FontStyle style = FontStyle.Regular;

            rtbLogs.SelectionStart = rtbLogs.TextLength;
            rtbLogs.SelectionLength = 0;

            rtbLogs.SelectionFont = new Font(rtbLogs.Font, style);
            rtbLogs.SelectionColor = Color.FromArgb(147, 157, 160);
            rtbLogs.AppendText(text1);
            rtbLogs.SelectionFont = new Font(rtbLogs.Font, FontStyle.Bold);
            rtbLogs.SelectionColor = color;
            rtbLogs.AppendText(resaltar);
            rtbLogs.SelectionFont = new Font(rtbLogs.Font, style);
            rtbLogs.SelectionColor = Color.FromArgb(147, 157, 160);
            rtbLogs.AppendText(text2);

            rtbLogs.AppendText("\n");

            rtbLogs.SelectionFont = new Font(rtbLogs.Font, FontStyle.Regular);
            rtbLogs.SelectionColor = rtbLogs.ForeColor;
            //rtbLogs.ScrollToCaret();
        }

        private void TittleLbl_Click(object sender, EventArgs e)
        {
            if (modCommon.Hackerprod)
            {
                Write(modCommon.sshClient.IsConnected, MessageType.INFO);
            }
        }




        private SshCommand sshCommand;

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottom(rtbLogs);
        }

        private void PingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool start = true;
            string server = modCommon.device.Server;

            while (start)
            {
                Ping ping = new Ping();
                int timeout = 100;
                PingOptions pingOption = new PingOptions(16, true);
                byte[] buffer = Encoding.ASCII.GetBytes("ping");
                try
                {
                    PingReply pingReply = ping.Send(server, timeout, buffer, pingOption);

                    if (pingReply.Status == IPStatus.Success)
                    {
                        PingLabel.Text = pingReply.RoundtripTime.ToString() + " ms";
                        Thread.Sleep(1000);
                        
                    }
                    else
                    {
                        PingLabel.Text = "Offline";
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                    PingLabel.Text = "Offline";
                    Thread.Sleep(1000);
                }

                if (pingWorker.CancellationPending)
                {
                    e.Cancel = true;
                    start = false;
                }
            }
        }

        private void RtbLogs_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void RtbLogs_Enter(object sender, EventArgs e)
        {
            DeviceLabel.Focus();
        }
        private void PowerBar_MouseMove(object sender, MouseEventArgs e)
        {
            CurrentPower.Text = PowerBar.Value.ToString();
        }

        private void SetChannel_Click(object sender, EventArgs e)
        {
            string channel = ChannelList.Text;
            string[] part = channel.Split(' ');
            channel = part[4];


            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }
            if (!string.IsNullOrEmpty(channel))
            {
                sshCommand = modCommon.sshClient.RunCommand("iwconfig ath0 channel " + channel + "&& save");
                Write("The device has switched to the channel " + channel);
            }
            modCommon.device.Channel = channel;
        }

        private void SetPower_Click(object sender, EventArgs e)
        {

            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }
            sshCommand = modCommon.sshClient.RunCommand("iwconfig ath0 txpower " + PowerBar.Value.ToString() + "&& save");
            sshCommand = modCommon.sshClient.RunCommand("save");
            sshCommand = modCommon.sshClient.RunCommand("sed -i '/radio.1.txpower/c\\radio.1.txpower=" + PowerBar.Value.ToString() + "' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save");

            Write("La potencia del equipo se ha cambiado a " + PowerBar.Value.ToString() + "dBm");

            modCommon.device.Power = PowerBar.Value.ToString();
        }

        private void AnchoChannel_Click(object sender, EventArgs e)
        {

        }

        private void DeviceLabel_TextChanged(object sender, EventArgs e)
        {
            DeviceLogo.Image = modCommon.GetDeviceImage(DeviceLabel.Text);
        }

        private void FrmManager_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void DeviceDetect_Click(object sender, EventArgs e)
        {
            frmDiscovery discovery = new frmDiscovery();
            discovery.Show();
        }

        private void RebootDevice_Click(object sender, EventArgs e)
        {
            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }
            sshCommand = modCommon.sshClient.RunCommand("reboot");
            Write("Restarting device", MessageType.WARN);
        }

        private void AddCT_Click(object sender, EventArgs e)
        {
            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }
            SshCommand sshCommand = modCommon.sshClient.RunCommand("echo '<option value='511' > Compliance Test</option>' >> /var/etc/ccodes.inc && cfgmtd -f /var/etc/ccodes.inc -w && save");

            Write("Compliance Test added to the list of countries", MessageType.INFO);

        }

        private void Cambiar_Click(object sender, EventArgs e)
        {
            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

            SshCommand sshCommand = modCommon.sshClient.RunCommand("grep 'users.1.name=' /tmp/system.cfg");
            string currentuser = sshCommand.Result.Replace("users.1.name=", "");
            currentuser = currentuser.Remove(currentuser.Length - 1, 1);
            Write(currentuser.Length);
            sshCommand = modCommon.sshClient.RunCommand("sed -i 's/users.1.name=" + currentuser + "/users.1.name=" + username.Text + "/g' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save");
            Write("Username changed, you must restart the device for the changes to take effect", MessageType.INFO);
            frmMain.frm.username.Text = username.Text;
            RegistrySettings.SaveSettings();
        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            List<int> channels = new List<int>();
            for (int i = 0; i < ChannelList.Items.Count; i++)
            {
                string channel = ChannelList.Items[i].ToString();
                string[] part = channel.Split(' ');
                channel = part[4];

                try
                {
                    channels.Add(Convert.ToInt32(channel));
                }
                catch { }
            }

            string chann = ChannelList.Text;
            string[] parts = chann.Split(' ');
            chann = parts[4];

            //new frmSearch().ShowDialog();
            new frmConnection(channels, chann).ShowDialog();
            
        }

        private void Label8_Click(object sender, EventArgs e)
        {
        }

        private void Stations_Click(object sender, EventArgs e)
        {
            new frmDevice().ShowDialog();
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
