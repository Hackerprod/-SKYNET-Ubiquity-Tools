using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualBasic;
using Renci.SshNet;
using System.Net.NetworkInformation;
using UbntDiscovery;

namespace SKYNET.GUI
{
    [ComVisibleAttribute(true)]
    public partial class frmMain : frmBase
    {
        public static frmMain frm;
        public static CT_Method CT_Method;
        public bool Searching;
        public DeviceDiscovery DeviceDiscovery;
        public StringBuilder HtmlString;

        private RegistrySettings settings;
        private bool connected;
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();

        public bool Connected
        {
            get { return connected; }
            set
            {
                if (value == true)
                {
                    connect.Text = "DISCONNECT";
                    this.StatusLabel.Text = "ONLINE";
                    this.StatusLabel.ForeColor = Color.Green;

                    if (!pingWorker.IsBusy)
                        pingWorker.RunWorkerAsync();

                    Common.SetVisibleControl(PingLabel, true);
                    Common.SetVisibleControl(lblping, true);
                    Common.SetVisibleControl(AdminDevice, true);

                    if (CountryLabel.Text != "COMPLIANCE TEST")
                    {
                        Common.SetVisibleControl(ActivateCT, true);
                    }


                }
                else 
                {
                    connect.Text = "CONNECT";

                    this.StatusLabel.Text = "OFFLINE";
                    this.StatusLabel.ForeColor = Color.Red;

                    DeviceLabel.Text = "Offline";
                    FirmwareLabel.Text = "Offline";
                    CountryLabel.Text = "Offline";
                    StatusLabel.Text = "Offline";

                    ActivateCT.Visible = false;
                    PingLabel.Visible = false;
                    lblping.Visible = false;
                    AdminDevice.Visible = false;
                    //modCommon.SetVisibleControl(PingLabel, false);
                    //modCommon.SetVisibleControl(lblping, false);
                    //modCommon.SetVisibleControl(AdminDevice, false);

                    pingWorker.CancelAsync();
                }
                connected = value;
                connect.Refresh();
            }
        }

        public frmMain()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;
            SetMouseMove(PN_Top);

            RegistrySettings.Initialice();
        }

        private void SshClient_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            Write(e.Exception.Message);
            Connected = false;
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
            RegistrySettings.SaveSettings();
            Process.GetCurrentProcess().Kill();
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
            Color color = Color.FromArgb(255, 255, 255);

            switch (type)
            {
                case MessageType.INFO:  color = Color.FromArgb(230, 230, 230); resaltar = "INFO    "; break;
                case MessageType.WARN:  color = ColorTranslator.FromHtml("#f58207"); resaltar = "WARN  "; break;
                case MessageType.ERROR: color = ColorTranslator.FromHtml("#f50729"); resaltar = "ERROR "; break;
                case MessageType.DEBUG: color = ColorTranslator.FromHtml("#07a4f5"); resaltar = "DEBUG "; break;
            }
            FontStyle style = FontStyle.Regular;

            rtbLogs.SelectionStart = rtbLogs.TextLength;
            rtbLogs.SelectionLength = 0;

            rtbLogs.SelectionFont = new Font(rtbLogs.Font, style);
            rtbLogs.SelectionColor = Color.FromArgb(245, 245, 245);
            rtbLogs.AppendText(text1);
            rtbLogs.SelectionFont = new Font(rtbLogs.Font, FontStyle.Bold);
            rtbLogs.SelectionColor = color;
            rtbLogs.AppendText(resaltar);
            rtbLogs.SelectionFont = new Font(rtbLogs.Font, style);
            rtbLogs.SelectionColor = Color.FromArgb(245, 245, 245);
            rtbLogs.AppendText(text2);

            rtbLogs.AppendText("\n");

            rtbLogs.SelectionFont = new Font(rtbLogs.Font, FontStyle.Regular);
            rtbLogs.SelectionColor = rtbLogs.ForeColor;
            //rtbLogs.ScrollToCaret();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Write("Connect to the device to get started");
            Connected = false;
            CT_Method = CT_Method.International;

            RegistrySettings.LoadSettings();

        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (connect.Text.ToLower() == "connect")
            {
                if (!IniciarSession.IsBusy)
                {
                    IniciarSession.RunWorkerAsync();
                }
            }
            else
            {
                Disconnect();
                Connected = false;
            }
        }

        private void IniciarSession_DoWork(object sender, DoWorkEventArgs e)
        {
            string Label1 = "";

            this.Disconnect();
            Write("Connecting to " + serverip.Text);
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(serverip.Text, 22, username.Text, password.Text);
            Common.sshClient = new SshClient(connectionInfo);
            Common.sshClient.ErrorOccurred += SshClient_ErrorOccurred;
            try
            {
                Common.sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10.0);
                Common.sshClient.ConnectionInfo.RetryAttempts = 3;
                Common.sshClient.Connect();
            }
            catch (Exception ex)
            {
                this.StatusLabel.Text = "ERROR!";
                this.StatusLabel.ForeColor = Color.Red;
            }
            try
            {
                if (Common.sshClient.IsConnected)
                {
                    Common.device = new Models.Device()
                    {
                        Server = serverip.Text,
                        Username = username.Text,
                        Password = password.Text
                    };

                    SshCommand sshCommand = Common.sshClient.RunCommand("cat /etc/version");
                    string result = sshCommand.Result;
                    if (result.Contains("-cs"))
                        try { result = result.Split('-')[0] + "-cs"; } catch { }

                    this.FirmwareLabel.Text = result;
                    sshCommand = Common.sshClient.RunCommand("cat /etc/board.info | grep board.name=");
                    Label1 = sshCommand.Result;
                    string text = Strings.Replace(Label1, "board.name=", "", 1, -1, CompareMethod.Binary);
                    this.DeviceLabel.Text = text;

                    sshCommand = Common.sshClient.RunCommand("cat /tmp/system.cfg | grep radio.countrycode=");
                    Label1 = sshCommand.Result;
                    string left = Strings.Replace(Label1, "radio.countrycode=", "", 1, -1, CompareMethod.Binary).Trim();

                    string country = Common.GetCountry(left);
                    CountryLabel.Text = country;
                    Common.SetVisibleControl(AdminDevice, true);

                    //////////////////////////////////////////////////////////////
                    {
                        //Obtener lista de canales
                        sshCommand = Common.sshClient.RunCommand("iwlist channel");
                        string channellist = sshCommand.Result;
                        TextBox box = new TextBox() { Multiline = true };
                        box.Text = channellist;
                        Common.device.ChannelList.Clear();
                        for (int i = 0; i < box.Lines.Count(); i++)
                        {
                            if (box.Lines[i].Contains(" Channel "))
                            {
                                Common.device.ChannelList.Add(box.Lines[i].Remove(0, 7));

                            }
                            if (box.Lines[i].Contains("Current Frequency:"))
                            {
                                string[] part = box.Lines[i].Split(' ');
                                for (int p = 0; p < part.Length; p++)
                                {
                                    if (part[p].Contains(")"))
                                    {
                                        Common.device.Channel = part[p].Replace(")", "");
                                    }
                                }

                            }
                        }
                    }
                    //Ports
                    {
                        sshCommand = Common.sshClient.RunCommand("grep 'httpd.https.port=' /tmp/system.cfg");
                        string https = sshCommand.Result;
                        Common.device.HttpsPort = https.Replace("httpd.https.port=", "");

                    }
                    {
                        //Obtener potencia del equipo
                        sshCommand = Common.sshClient.RunCommand("iwlist ath0 txpower");
                        string powerlist = sshCommand.Result;
                        TextBox box = new TextBox() { Multiline = true };
                        box.Text = powerlist;
                        List<string> dbm = new List<string>();
                        string Max = "";
                        for (int i = 0; i < box.Lines.Count(); i++)
                            if (box.Lines[i].Contains("dBm")) dbm.Add(box.Lines[i].ToString());

                        //Potencia maxima
                        if (dbm.Count > 2)
                        {
                            Max = dbm[dbm.Count - 2];
                            string[] part = Max.Split(' ');
                            Max = part[2];
                        }
                        Common.device.MaxPower = Max;


                        //Potencia actual
                        string power = "";
                        if (dbm.Count > 2)
                        {
                            power = dbm[dbm.Count - 1];
                            string[] part = power.Split(' ');
                            for (int i = 0; i < part.Length; i++)
                            {
                                if (part[i].Contains("="))
                                {
                                    power = part[i].Replace("Tx-Power=", "");
                                }
                            }
                        }
                        Common.device.Power = power;
                    }
                    Connected = true;

                }
                else
                {
                    Connected = false;
                }
            }
            catch (Exception)
            {
                Write("An error occurred while establishing the connection");
            }

            while (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }
            if (Connected)
                Write("Established connection");
        }

        private void ActivateCT_Click(object sender, EventArgs e)
        {
            if (Connected)
            {

                if (CTWorker.IsBusy)
                    return;

                frmComplianceTest frmCompliance = new frmComplianceTest();
                frmCompliance.ShowDialog();

                CTWorker.RunWorkerAsync();
            }
        }

        private void Disconnect()
        {
            bool showMessage = false;
            if (this.Connected)
            {
                if (Common.sshClient.IsConnected)
                {
                    Common.sshClient.Disconnect();
                    Common.sshClient.Dispose();
                    showMessage = true;
                }
            }
            this.Connected = false;
            if (showMessage) Write("You have disconnected from your device");
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottom(rtbLogs);
        }


        private void PingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool start = true;
            string server = serverip.Text;

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
            connect.Focus();
        }

        private void CTWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            if (this.FirmwareLabel.Text.Contains("XC"))
            {
                MessageBox.Show("The equipment is not from the Ubiquiti airMAX M Line!.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Write("Error! Detected as Airmax AC device.", MessageType.ERROR);
                this.Write("XC firmware not supported.", MessageType.ERROR);
                return;
            }
            if (this.FirmwareLabel.Text.Contains("WA"))
            {
                MessageBox.Show("The equipment is not from the Ubiquiti airMAX M Line!.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Write("Error! Detected as Airmax AC device.", MessageType.ERROR);
                this.Write("WA firmware not supported.", MessageType.ERROR);
                return;
            }

            while (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }

            this.Write("Trying to activate the Compliance Test to the device", MessageType.INFO);

            var script = Common.GetComplianceTestScript(CT_Method);

            foreach (var line in script)
            {
                SshCommand result = Common.sshClient.RunCommand(line);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    //Write($"Error in command {line} : {result.Error}");
                }
            }

            //sshCommand = modCommon.sshClient.RunCommand(modCommon.GetChannelsCode());

            if (!CountryCodeWorker.IsBusy)
                CountryCodeWorker.RunWorkerAsync();
        }

        private void CountryCodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!Common.sshClient.IsConnected)
                {
                    try
                    {
                        Common.sshClient.Connect();
                    }
                    catch { }
                }
                try
                {
                    using (Common.sshClient)
                    {
                        if (Common.sshClient.IsConnected)
                        {
                            string Label1 = "";
                            SshCommand sshCommand = Common.sshClient.RunCommand("cat /tmp/system.cfg | grep radio.countrycode=");
                            Label1 = sshCommand.Result;
                            string left = Strings.Replace(Label1, "radio.countrycode=", "", 1, -1, CompareMethod.Binary).Trim();

                            string pais = Common.GetCountry(left);
                            CountryLabel.Text = pais;

                            if (pais == "COMPLIANCE TEST")
                            {
                                this.Write("Done ... Compliance Test activated.", MessageType.INFO);

                                if (CT_Method == CT_Method.USA)
                                {
                                    this.Write("Restarting the device.", MessageType.WARN);
                                    Connected = false;
                                }
                                Common.SetVisibleControl(ActivateCT, false);
                                goto label_1;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            label_1:;
        }

        private void AdminDevice_Click(object sender, EventArgs e)
        {
            frmManager manager = new frmManager();
            manager.ShowDialog();
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                if (connect.Text.ToLower() == "connect")
                {
                    if (!IniciarSession.IsBusy)
                    {
                        IniciarSession.RunWorkerAsync();
                    }
                }
                else
                {
                    Disconnect();
                    Connected = false;
                }
                e.SuppressKeyPress = true;
            }
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            Visible = true;
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            
        }

        internal void SetEquipo(string ipName)
        {
            serverip.Text = ipName;
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            while (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }

            this.Write("Testing", MessageType.INFO);


            SshCommand sshCommand = Common.sshClient.RunCommand("sed -i '/radio.countrycode/c\\radio.countrycode=511' /tmp/system.cfg && sed -i '/radio.1.countrycode/c\\radio.1.countrycode=511' /tmp/system.cfg");
            sshCommand = Common.sshClient.RunCommand("sed -i '/radio.countrycode/c\\radio.countrycode=511' /tmp/system.cfg && sed -i '/radio.1.countrycode/c\\radio.1.countrycode=511' /tmp/system.cfg");

            sshCommand = Common.sshClient.RunCommand("echo \"countrycode = 511\" > /var/etc/atheros.conf");
            sshCommand = Common.sshClient.RunCommand("sed -i 's/840/511/g' /tmp/system.cfg");
            sshCommand = Common.sshClient.RunCommand("<option value=\"511\"> Compliance Test</option>\" >> /var/etc/ccodes.inc");
            sshCommand = Common.sshClient.RunCommand("cfgmtd -f /tmp/system.cfg -w");
            sshCommand = Common.sshClient.RunCommand("cfgmtd -f /var/etc/ccodes.inc -w");
            sshCommand = Common.sshClient.RunCommand("cfgmtd -f /var/etc/atheros.conf -w");
            sshCommand = Common.sshClient.RunCommand("save");
  
            sshCommand = Common.sshClient.RunCommand("cat /tmp/system.cfg | grep countrycode");
            this.Write(sshCommand.Result, MessageType.INFO);
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

        private void SKYNET_Button1_Click(object sender, EventArgs e)
        {
            frmDiscovery discovery = new frmDiscovery();
            discovery.ShowDialog();
        }

        private void FlatButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
