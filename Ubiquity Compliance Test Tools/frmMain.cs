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
using UbntDiscovery;

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmMain : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmMain frm;
        private static ILog ilog_0;

        public DeviceDiscovery DeviceDiscovery { get; }
        RegistrySettings settings;
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
                    connect.Text = "DISCONNECT";
                    this.StatusLabel.Text = "ONLINE";
                    this.StatusLabel.ForeColor = Color.Green;

                    if (!pingWorker.IsBusy)
                        pingWorker.RunWorkerAsync();

                    modCommon.SetVisibleControl(PingLabel, true);
                    modCommon.SetVisibleControl(lblping, true);
                    modCommon.SetVisibleControl(AdminDevice, true);

                    if (CountryLabel.Text != "COMPLIANCE TEST")
                    {
                        modCommon.SetVisibleControl(ActivateCT, true);
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

            //Initialize class
            ilog_0 = new ILog();
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

        private void TittleLbl_Click(object sender, EventArgs e)
        {
            if (modCommon.Hackerprod)
            {
                List<string> Users = new List<string>()
                {
                    "ubnt",
                    "Sluis",
                    "UBNT",
                    "Ubnt",
                    "SKYNET",
                    "skynet",
                    "Skynet",
                    "Hackerprod",
                    "hackerprod"
                };
                List<string> Passwords = new List<string>()
                {
                    "dlh8904",
                    "dlh8904*",
                    "dlh890415",
                    "dlh890415*",
                    "loops8904",
                    "loops8904*",
                    "loops890415",
                    "loops890415*",
                    "dlh89041515140",
                    "admin456*",
                    "Admin456*",

                    "123",
                    "1234",
                    "12345",
                    "123456",
                    "123457",
                    "1234578",
                    "12345789",
                    "123457890",
                    "123*",
                    "1234*",
                    "12345*",
                    "123456*",
                    "123457*",
                    "1234578*",
                    "12345789*",
                    "123457890*",


                    "Dlh8904",
                    "Dlh8904*",
                    "Dlh890415",
                    "Dlh890415*",
                    "Loops8904",
                    "Loops8904*",
                    "Loops890415",
                    "Loops890415*",

                    "skynet123",
                    "Skynet123",
                    "skynet123*",
                    "Skynet123*",
                    "skynet8904",
                    "Skynet8904*",
                    "skynet890415",
                    "Skynet890415",
                    "skynet890415*",
                    "Skynet890415*",

                };
                foreach (var User_Current in Users)
                {
                    foreach (var Password_Current in Passwords)
                    {
                        Write("Connecting with user: " + User_Current + ", password: " + Password_Current, MessageType.WARN);
                        if (IniciarSession_Test(User_Current, Password_Current))
                        {
                            modCommon.Show("Connected with user: " + User_Current + ", password: " + Password_Current);
                        }
                    }
                }
                

                



                return;
                SshCommand sshCommand = modCommon.sshClient.RunCommand("save");
                string command = "";
                //sshCommand = modCommon.sshClient.RunCommand("cd /etc/persistent && touch rc.poststart && chmod +x rc.poststart");

                //Remove old file
                sshCommand = modCommon.sshClient.RunCommand("rm /etc/persistent");

                //Create new
                sshCommand = modCommon.sshClient.RunCommand("cd /etc/persistent && touch rc.poststart && chmod +x rc.poststart");

                //Edit new file
                command += "sed -i 'A#!/bin/sh' /etc/persistent/rc.poststart && ";
                /*command += "sed -i 'Ainsmod ubnt_spectral' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aiwpriv wifi0 setCountry UB' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aifconfig ath0 down' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aifconfig wifi0 down' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Asleep 5' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Armmod ubnt_spectral' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aifconfig ath0 up' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aifconfig wifi0 up' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aecho \"countrycode = 511\" > /var/etc/atheros.conf' /etc/persistent/rc.poststart && ";
                command += "sed -i 'A' /etc/persistent/rc.poststart && ";
                command += "sed -i 'A' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Ased -i 's/840/511/g' /tmp/system.cfg' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Aecho \"<option value=\"511\" > Compliance Test</option>\" >> /var/etc/ccodes.inc' /etc/persistent/rc.poststart && ";
                command += "sed -i 'Asleep 5' /etc/persistent/rc.poststart";
                */
                command +=   "cfgmtd -f -w && save";
                
                sshCommand = modCommon.sshClient.RunCommand(command);

                sshCommand = modCommon.sshClient.RunCommand("cat /etc/persistent/rc.poststart");

                Write(sshCommand.Result);
                

                //sshCommand = modCommon.sshClient.RunCommand("cd /etc/persistent && touch rc.poststart && chmod +x rc.poststart");
                //sshCommand = modCommon.sshClient.RunCommand("touch /etc/persistent/rc.poststart && sed -i '#!/bin/sh' /etc/persistent/rc.poststart && sed -i 'insmod ubnt_spectral' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                /*
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'iwpriv wifi0 setCountry UB' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'ifconfig ath0 down' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'ifconfig wifi0 down' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'sleep 5' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'rmmod ubnt_spectral' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'ifconfig ath0 up' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'ifconfig wifi0 up' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'echo \"countrycode=511\" > /var/etc/atheros.conf' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 's/840/511/g /tmp/system.cfg' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'echo \"<option value=\"511\">Compliance Test</option>\" >> /var/etc/ccodes.inc' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                sshCommand = modCommon.sshClient.RunCommand("sed -i 'sleep 5' /etc/persistent/rc.poststart && cfgmtd -f /etc/persistent/rc.poststart -w && save");
                */


                




            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ilog_0.Info("Connect to the device to get started");
            Connected = false;
            Common.Metodo = Common.Method.Method_1;

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
        private bool IniciarSession_Test(string user, string password)
        {
            string Label1 = "";

            this.Disconnect();
            Write("Connecting to " + serverip.Text);
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(serverip.Text, 22, user, password);
            modCommon.sshClient = new SshClient(connectionInfo);
            modCommon.sshClient.ErrorOccurred += SshClient_ErrorOccurred;
            try
            {
                modCommon.sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10.0);
                modCommon.sshClient.ConnectionInfo.RetryAttempts = 3;
                modCommon.sshClient.Connect();

            }
            catch (Exception ex)
            {
                this.StatusLabel.Text = "ERROR!";
                this.StatusLabel.ForeColor = Color.Red;
            }
            try
            {
                if (modCommon.sshClient.IsConnected)
                {
                    modCommon.device = new Device();
                    modCommon.device.Server = serverip.Text;
                    modCommon.device.Username = user;
                    modCommon.device.Password = password;

                    SshCommand sshCommand = modCommon.sshClient.RunCommand("cat /etc/version");
                    string result = sshCommand.Result;
                    if (result.Contains("-cs"))
                        try { result = result.Split('-')[0] + "-cs"; } catch { }

                    this.FirmwareLabel.Text = result;
                    sshCommand = modCommon.sshClient.RunCommand("cat /etc/board.info | grep board.name=");
                    Label1 = sshCommand.Result;
                    string text = Strings.Replace(Label1, "board.name=", "", 1, -1, CompareMethod.Binary);
                    this.DeviceLabel.Text = text;

                    sshCommand = modCommon.sshClient.RunCommand("cat /tmp/system.cfg | grep radio.countrycode=");
                    Label1 = sshCommand.Result;
                    string left = Strings.Replace(Label1, "radio.countrycode=", "", 1, -1, CompareMethod.Binary).Trim();

                    //sshCommand = sshClient.RunCommand("iwconfig ath0 txpower 20");

                    string pais = modCommon.GetCountry(left);
                    CountryLabel.Text = pais;
                    modCommon.SetVisibleControl(AdminDevice, true);

                    //////////////////////////////////////////////////////////////
                    {
                        //Obtener lista de canales
                        sshCommand = modCommon.sshClient.RunCommand("iwlist channel");
                        string channellist = sshCommand.Result;
                        TextBox box = new TextBox() { Multiline = true };
                        box.Text = channellist;
                        modCommon.device.ChannelList.Clear();
                        for (int i = 0; i < box.Lines.Count(); i++)
                        {
                            if (box.Lines[i].Contains(" Channel "))
                            {
                                modCommon.device.ChannelList.Add(box.Lines[i].Remove(0, 7));

                            }
                            if (box.Lines[i].Contains("Current Frequency:"))
                            {
                                string[] part = box.Lines[i].Split(' ');
                                for (int p = 0; p < part.Length; p++)
                                {
                                    if (part[p].Contains(")"))
                                    {
                                        modCommon.device.Channel = part[p].Replace(")", "");
                                    }
                                }

                            }
                        }
                    }
                    //Ports
                    {
                        sshCommand = modCommon.sshClient.RunCommand("grep 'httpd.https.port=' /tmp/system.cfg");
                        string https = sshCommand.Result;
                        modCommon.device.HttpsPort = https.Replace("httpd.https.port=", "");

                    }
                    {
                        //Obtener potencia del equipo
                        sshCommand = modCommon.sshClient.RunCommand("iwlist ath0 txpower");
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
                        modCommon.device.MaxPower = Max;


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
                        modCommon.device.Power = power;
                    }
                    Connected = true;
                    return true;
                }
                else
                {
                    Connected = false;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void IniciarSession_DoWork(object sender, DoWorkEventArgs e)
        {
            string Label1 = "";

            this.Disconnect();
            Write("Connecting to " + serverip.Text);
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(serverip.Text, 22, username.Text, password.Text);
            modCommon.sshClient = new SshClient(connectionInfo);
            modCommon.sshClient.ErrorOccurred += SshClient_ErrorOccurred;
            try
            {
                modCommon.sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10.0);
                modCommon.sshClient.ConnectionInfo.RetryAttempts = 3;
                modCommon.sshClient.Connect();
            }
            catch (Exception ex)
            {
                this.StatusLabel.Text = "ERROR!";
                this.StatusLabel.ForeColor = Color.Red;
            }
            try
            {
                if (modCommon.sshClient.IsConnected)
                {
                    modCommon.device = new Device();
                    modCommon.device.Server = serverip.Text;
                    modCommon.device.Username = username.Text;
                    modCommon.device.Password = password.Text;

                    SshCommand sshCommand = modCommon.sshClient.RunCommand("cat /etc/version");
                    string result = sshCommand.Result;
                    if (result.Contains("-cs"))
                        try { result = result.Split('-')[0] + "-cs"; } catch { }

                    this.FirmwareLabel.Text = result;
                    sshCommand = modCommon.sshClient.RunCommand("cat /etc/board.info | grep board.name=");
                    Label1 = sshCommand.Result;
                    string text = Strings.Replace(Label1, "board.name=", "", 1, -1, CompareMethod.Binary);
                    this.DeviceLabel.Text = text;

                    sshCommand = modCommon.sshClient.RunCommand("cat /tmp/system.cfg | grep radio.countrycode=");
                    Label1 = sshCommand.Result;
                    string left = Strings.Replace(Label1, "radio.countrycode=", "", 1, -1, CompareMethod.Binary).Trim();

                    string country = modCommon.GetCountry(left);
                    CountryLabel.Text = country;
                    modCommon.SetVisibleControl(AdminDevice, true);

                    //////////////////////////////////////////////////////////////
                    {
                        //Obtener lista de canales
                        sshCommand = modCommon.sshClient.RunCommand("iwlist channel");
                        string channellist = sshCommand.Result;
                        TextBox box = new TextBox() { Multiline = true };
                        box.Text = channellist;
                        modCommon.device.ChannelList.Clear();
                        for (int i = 0; i < box.Lines.Count(); i++)
                        {
                            if (box.Lines[i].Contains(" Channel "))
                            {
                                modCommon.device.ChannelList.Add(box.Lines[i].Remove(0, 7));

                            }
                            if (box.Lines[i].Contains("Current Frequency:"))
                            {
                                string[] part = box.Lines[i].Split(' ');
                                for (int p = 0; p < part.Length; p++)
                                {
                                    if (part[p].Contains(")"))
                                    {
                                        modCommon.device.Channel = part[p].Replace(")", "");
                                    }
                                }

                            }
                        }
                    }
                    //Ports
                    {
                        sshCommand = modCommon.sshClient.RunCommand("grep 'httpd.https.port=' /tmp/system.cfg");
                        string https = sshCommand.Result;
                        modCommon.device.HttpsPort = https.Replace("httpd.https.port=", "");

                    }
                    {
                        //Obtener potencia del equipo
                        sshCommand = modCommon.sshClient.RunCommand("iwlist ath0 txpower");
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
                        modCommon.device.MaxPower = Max;


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
                        modCommon.device.Power = power;
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
                ilog_0.Error("An error occurred while establishing the connection");
            }

            while (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
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
                if (modCommon.sshClient.IsConnected)
                {
                    modCommon.sshClient.Disconnect();
                    modCommon.sshClient.Dispose();
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

            while (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

            this.Write("Trying to activate the Compliance Test to the device", MessageType.INFO);
            if (Common.Metodo == Common.Method.Method_1)
            {
                SshCommand sshCommand = modCommon.sshClient.RunCommand(modCommon.GetCTCode_M1());
            }
            else if (Common.Metodo == Common.Method.Method_2)
            {
                SshCommand sshCommand = modCommon.sshClient.RunCommand(modCommon.GetCTCode_M2());
                sshCommand = modCommon.sshClient.RunCommand(modCommon.GetActivateListChannelsCode());
                sshCommand = modCommon.sshClient.RunCommand(modCommon.GetListChannelsCode());
                sshCommand = modCommon.sshClient.RunCommand("save");
                sshCommand = modCommon.sshClient.RunCommand("cfgmtd -wp /etc && reboot");
                sshCommand = modCommon.sshClient.RunCommand("reboot");
            }

            //sshCommand = modCommon.sshClient.RunCommand(modCommon.GetChannelsCode());

            if (!CountryCodeWorker.IsBusy)
                CountryCodeWorker.RunWorkerAsync();
        }

        private void CountryCodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!modCommon.sshClient.IsConnected)
                {
                    try
                    {
                        modCommon.sshClient.Connect();
                    }
                    catch { }
                }
                try
                {
                    using (modCommon.sshClient)
                    {
                        if (modCommon.sshClient.IsConnected)
                        {
                            string Label1 = "";
                            SshCommand sshCommand = modCommon.sshClient.RunCommand("cat /tmp/system.cfg | grep radio.countrycode=");
                            Label1 = sshCommand.Result;
                            string left = Strings.Replace(Label1, "radio.countrycode=", "", 1, -1, CompareMethod.Binary).Trim();

                            string pais = modCommon.GetCountry(left);
                            CountryLabel.Text = pais;

                            if (pais == "COMPLIANCE TEST")
                            {
                                this.Write("Done ... Compliance Test activated.", MessageType.INFO);

                                if (Common.Metodo == Common.Method.Method_2)
                                {
                                    this.Write("Restarting the device.", MessageType.WARN);
                                    Connected = false;
                                }
                                modCommon.SetVisibleControl(ActivateCT, false);
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

        private void Descovery_Click(object sender, EventArgs e)
        {
            frmDiscovery discovery = new frmDiscovery();
            discovery.ShowDialog();
        }

        private void Descovery_MouseMove(object sender, MouseEventArgs e)
        {
            Descovery.ForeColor = Color.FromArgb(225, 225, 225);
        }

        private void Descovery_MouseLeave(object sender, EventArgs e)
        {
            Descovery.ForeColor = Color.FromArgb(147, 157, 160);
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            while (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

            this.Write("Testing", MessageType.INFO);


            SshCommand sshCommand = modCommon.sshClient.RunCommand("sed -i '/radio.countrycode/c\\radio.countrycode=511' /tmp/system.cfg && sed -i '/radio.1.countrycode/c\\radio.1.countrycode=511' /tmp/system.cfg");
            sshCommand = modCommon.sshClient.RunCommand("sed -i '/radio.countrycode/c\\radio.countrycode=511' /tmp/system.cfg && sed -i '/radio.1.countrycode/c\\radio.1.countrycode=511' /tmp/system.cfg");

            sshCommand = modCommon.sshClient.RunCommand("echo \"countrycode = 511\" > /var/etc/atheros.conf");
            sshCommand = modCommon.sshClient.RunCommand("sed -i 's/840/511/g' /tmp/system.cfg");
            sshCommand = modCommon.sshClient.RunCommand("<option value=\"511\"> Compliance Test</option>\" >> /var/etc/ccodes.inc");
            sshCommand = modCommon.sshClient.RunCommand("cfgmtd -f /tmp/system.cfg -w");
            sshCommand = modCommon.sshClient.RunCommand("cfgmtd -f /var/etc/ccodes.inc -w");
            sshCommand = modCommon.sshClient.RunCommand("cfgmtd -f /var/etc/atheros.conf -w");
            sshCommand = modCommon.sshClient.RunCommand("save");
  
            sshCommand = modCommon.sshClient.RunCommand("cat /tmp/system.cfg | grep countrycode");
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

    }
}
