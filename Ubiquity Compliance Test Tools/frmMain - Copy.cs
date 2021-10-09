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
using SKYNET.Common;
using SKYNET.LOG;
using SBSSHKeyStorage;
using SBSharedResource;
using SBSimpleSSH;
using System.Timers;
using SBSSHCommon;
using SBUtils;
using SBStringList;
using Microsoft.VisualBasic;
using Renci.SshNet;
using System.Net.NetworkInformation;
using SBSocket;

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
                    connect.Text = "DESCONECTAR";
                    this.StatusLabel.Text = "CONECTADO";
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
                    connect.Text = "CONECTAR";

                    this.StatusLabel.Text = "DESCONECTADO";
                    this.StatusLabel.ForeColor = Color.Red;

                    DeviceLabel.Text = "Desconectado";
                    FirmwareLabel.Text = "Desconectado";
                    CountryLabel.Text = "Desconectado";
                    StatusLabel.Text = "Desconectado";

                    modCommon.SetVisibleControl(ActivateCT, false);
                    modCommon.SetVisibleControl(PingLabel, false);
                    modCommon.SetVisibleControl(lblping, false);
                    modCommon.SetVisibleControl(AdminDevice, false);

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
                        case "ClosePic": CloseBox.BackColor = Color.FromArgb(57, 62, 63); break;
                        case "MinPic": MinBox.BackColor = Color.FromArgb(57, 62, 63); break;
                    }
                }
                if (control is Panel)
                {
                    switch (control.Name)
                    {
                        case "CloseBox": CloseBox.BackColor = Color.FromArgb(57, 62, 63); break;
                        case "MinBox": MinBox.BackColor = Color.FromArgb(57, 62, 63); break;
                    }
                }
            }   catch { }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            MinBox.BackColor = Color.FromArgb(43, 47, 48);
            CloseBox.BackColor = Color.FromArgb(43, 47, 48);
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
                case MessageType.WARN:  color = ColorTranslator.FromHtml("#f58207"); resaltar = "WARN    "; break;
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
                /*serverip.Text = "10.31.0.200";
                username.Text = "Yohel";
                password.Text = "c00º";*/
                SshCommand sshCommand = modCommon.sshClient.RunCommand(modCommon.GetCTCode());
                Write(sshCommand.Result);
            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ilog_0.Info("Conéctese al equipo para comenzar");
            Connected = false;

            if (modCommon.Hackerprod)
            {
                serverip.Text = "10.31.0.202";
                username.Text = "Hackerprod";
                password.Text = "dlh8904";
            }

            InitializeSSHClient();

        }
        private void InitializeSSHClient()
        {
            SimpleSSHClient = new TElSimpleSSHClient();
            SBUtils.__Global.SetLicenseKey("239E70BC3370F136EC9CFB5750A702B94DB078754A94E95FE45B29E2AF2CD86D4CA4FCE1B2D9888EB827F1E01294824D465730895F244E7DE61B50342857C6859B83DDD55919A1BB152DC8C90E43B312A890CD1C85A266A8BF0BB05A4496C9688F5D8D1AB3FE05133843401E51E12056B3FA6AAE876FFA83EC15A5F311EF1D422858F9774C28A639948D7930AD113DD65972A4BAB19F6246FB50C37F585A84C0D872212BCCAC88BC5082E32F4AC4EEA3A779F7EB4251DA090BA524B112F5D684BA9E45C2BE71C02A5485DD8BBAE0091419C34A73FD7B3B0248C8AC9D2B8ADAFD5A90D3739B7B715C4517F08A851FC3825ACA6609421811EA214B7FA997C01597");
            this.SSHMemoryKeyStorage = new TElSSHMemoryKeyStorage();
            this.SimpleSSHClient.KeyStorage = this.SSHMemoryKeyStorage;
            this.InitializeSimpleSSHClient(new TElSimpleSSHClient());

            TElDNSSettings telDNSSettings = new TElDNSSettings();
            telDNSSettings.AllowStatuses = 11;
            telDNSSettings.Port = 53;
            telDNSSettings.QueryTimeout = 3000;
            telDNSSettings.TotalTimeout = 15000;
            this.SimpleSSHClient.DNS = telDNSSettings;
            this.SimpleSSHClient.ForceCompression = false;
            this.SimpleSSHClient.GSSDelegateCredentials = false;
            this.SimpleSSHClient.GSSHostName = "";
            this.SimpleSSHClient.GSSMechanism = null;
            this.SimpleSSHClient.IncomingSpeedLimit = 0;
            this.SimpleSSHClient.KeyStorage = null;
            this.SimpleSSHClient.LocalAddress = "";
            this.SimpleSSHClient.LocalPort = 0;
            this.SimpleSSHClient.MaxSSHPacketSize = 262144;
            this.SimpleSSHClient.MinWindowSize = 2048;
            this.SimpleSSHClient.OutgoingSpeedLimit = 0;
            this.SimpleSSHClient.Password = "";
            this.SimpleSSHClient.Port = 22;
            this.SimpleSSHClient.PortKnock = null;
            this.SimpleSSHClient.RaiseExceptionOnCommandTimeout = false;
            this.SimpleSSHClient.RaiseExceptionOnTunnelFailure = false;
            this.SimpleSSHClient.RequestPasswordChange = false;
            this.SimpleSSHClient.RequestTerminal = true;
            this.SimpleSSHClient.SendCommandEOF = false;
            this.SimpleSSHClient.SocketTimeout = 60000;
            this.SimpleSSHClient.SocksAuthentication = 0;
            this.SimpleSSHClient.SocksPassword = null;
            this.SimpleSSHClient.SocksPort = 0;
            this.SimpleSSHClient.SocksResolveAddress = false;
            this.SimpleSSHClient.SocksServer = null;
            this.SimpleSSHClient.SocksUseIPv6 = false;
            this.SimpleSSHClient.SocksUserCode = null;
            this.SimpleSSHClient.SocksVersion = 0;
            this.SimpleSSHClient.SoftwareName = "EldoS.SSHBlackbox.8";
            this.SimpleSSHClient.SSHAuthOrder = TSBSSHAuthOrder.aoDefault;
            this.SimpleSSHClient.Subsystem = null;
            this.SimpleSSHClient.TerminalInfo = null;
            this.SimpleSSHClient.ThrottleControl = true;
            this.SimpleSSHClient.TrustedKeys = null;
            this.SimpleSSHClient.UseInternalSocket = true;
            this.SimpleSSHClient.UseIPv6 = false;
            this.SimpleSSHClient.Username = "";
            this.SimpleSSHClient.UseSocks = false;
            this.SimpleSSHClient.UseUTF8 = false;
            this.SimpleSSHClient.UseWebTunneling = false;
            this.SimpleSSHClient.Versions = 0;
            this.SimpleSSHClient.WebTunnelAddress = null;
            this.SimpleSSHClient.WebTunnelAuthentication = 0;
            this.SimpleSSHClient.WebTunnelPassword = null;
            this.SimpleSSHClient.WebTunnelPort = 0;
            this.SimpleSSHClient.WebTunnelUserId = null;
        }

        internal virtual void InitializeSimpleSSHClient(TElSimpleSSHClient WithEventsValue)
        {
            TSSHAuthenticationFailedEvent value = new TSSHAuthenticationFailedEvent(this.OnAuthenticationFailed);
            TNotifyEvent value2 = new TNotifyEvent(this.OnAuthenticationSuccess);
            TSSHCloseConnectionEvent value3 = new TSSHCloseConnectionEvent(this.OnCloseConnection);
            TSSHErrorEvent value4 = new TSSHErrorEvent(this.OnError);
            TSSHKeyValidateEvent value5 = new TSSHKeyValidateEvent(this.OnKeyValidate);
            TSSHAuthenticationKeyboardEvent value6 = new TSSHAuthenticationKeyboardEvent(this.OnAuthenticationKeyboard);
            TElSimpleSSHClient telSimpleSSHClient = this.SimpleSSHClient;
            if (telSimpleSSHClient != null)
            {
                telSimpleSSHClient.OnAuthenticationFailed -= value;
                telSimpleSSHClient.OnAuthenticationSuccess -= value2;
                telSimpleSSHClient.OnCloseConnection -= value3;
                telSimpleSSHClient.OnError -= value4;
                telSimpleSSHClient.OnKeyValidate -= value5;
                telSimpleSSHClient.OnAuthenticationKeyboard -= value6;
            }
            this.SimpleSSHClient = WithEventsValue;
            telSimpleSSHClient = this.SimpleSSHClient;
            if (telSimpleSSHClient != null)
            {
                telSimpleSSHClient.OnAuthenticationFailed += value;
                telSimpleSSHClient.OnAuthenticationSuccess += value2;
                telSimpleSSHClient.OnCloseConnection += value3;
                telSimpleSSHClient.OnError += value4;
                telSimpleSSHClient.OnKeyValidate += value5;
                telSimpleSSHClient.OnAuthenticationKeyboard += value6;
            }
        }


        private void OnAuthenticationFailed(object object_0, int int_2)
        {
            this.Write("Autenticación fallida, tipo (" + Conversions.ToString(int_2) + ").", MessageType.ERROR);
        }

        private void OnAuthenticationSuccess(object object_0)
        {
            this.Write("Se ha autenticado correctamente", MessageType.INFO);
        }

        private void OnCloseConnection(object object_0)
        {
            this.timer_1.Enabled = false;
            this.Write("Conexión cerrada.", MessageType.DEBUG);
        }

        private void OnError(object object_0, int int_2)
        {
            this.timer_1.Enabled = false;
            //this.Write("Error " + Conversions.ToString(int_2), MessageType.ERROR);
        }

        private void OnKeyValidate(object object_0, TElSSHKey telSSHKey_0, ref bool bool_1)
        {
            //this.Write("Server key received, fingerprint " + SBUtils.__Global.DigestToStr128(telSSHKey_0.FingerprintMD5, true), false);
            bool_1 = true;
        }
        private void OnAuthenticationKeyboard(object Sender, TElStringList Prompts, bool[] Echo, TElStringList Responses)
        {
            Prompts.Clear();
            checked
            {
                int num = Prompts.Count - 1;
                for (int i = 0; i <= num; i++)
                {
                    string value = "";
                    /*if (Prompt(Prompts[i], Echo[i], ref value))
                    {
                        //Prompts.Add(value);
                    }
                    else
                    {
                        //Prompts.Add("");
                    }*/
                }
            }
        }

        private TElSSHMemoryKeyStorage SSHMemoryKeyStorage;
        private TElSimpleSSHClient SimpleSSHClient;


        private void Connect_Click(object sender, EventArgs e)
        {
            if (connect.Text.ToLower() == "conectar")
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
            bool connected1 = false;
            Write("Conectando a " + serverip.Text);
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo(serverip.Text, 22, username.Text, password.Text);
            modCommon.sshClient = new SshClient(connectionInfo);
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
                    this.FirmwareLabel.Text = sshCommand.Result;
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
                    connected1 = true;
                }
                else
                {
                    Connected = false;
                }
            }
            catch (Exception)
            {
                ilog_0.Error("Ha ocurrido un error al establecer la conexión");
            }

            //////////////////////////////////////////////////////////////
            this.Write("Estableciendo conexión SSH", MessageType.INFO);
            bool cbSSH1 = true;
            bool cbSSH2 = true;

            this.SimpleSSHClient.Address = serverip.Text;
            this.SimpleSSHClient.Port = 22;
            this.SimpleSSHClient.Username = username.Text;
            this.SimpleSSHClient.Password = password.Text;
            this.SimpleSSHClient.Versions = 0;

            if (cbSSH1)
            {
                this.SimpleSSHClient.Versions = (short)(this.SimpleSSHClient.Versions | 1);
            }
            if (cbSSH2)
            {
                this.SimpleSSHClient.Versions = (short)(this.SimpleSSHClient.Versions | 2);
            }

            this.SimpleSSHClient.ForceCompression = true;
            this.SimpleSSHClient.AuthenticationTypes = 20;
            this.SSHMemoryKeyStorage.Clear();

            this.SimpleSSHClient.AuthenticationTypes = (this.SimpleSSHClient.AuthenticationTypes & -3);

            try
            {
                this.SimpleSSHClient.Open();
                this.Connected = this.SimpleSSHClient.Active;
                Connected = true;
                this.Write("Conexión establecida", MessageType.INFO);
            }
            catch (Exception ex)
            {
                this.Write(ex.Message, MessageType.ERROR);
                if (this.SimpleSSHClient.ServerSoftwareName.Length > 0)
                {
                    this.Write("El servidor se identificó como: " + this.SimpleSSHClient.ServerSoftwareName, MessageType.ERROR);
                }
                try
                {
                    this.SimpleSSHClient.Close(true);
                }
                catch (Exception)
                {
                }
                return;
            }
            if (connected1 == true)
                Connected = true;

            this.timer_1.Enabled = true;
            while (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

        }

        private void ActivateCT_Click(object sender, EventArgs e)
        {
            if (Connected)
            {

                if (CTWorker.IsBusy)
                    return;

                CTWorker.RunWorkerAsync();
            }
        }

        private void method_55(object sender, EventArgs e)
        {
            bool flag = true;
            while (flag)
            {
                try
                {
                    flag = this.SimpleSSHClient.CanReceive(0);
                }
                catch (Exception ex)
                {
                    flag = false;
                }
                if (flag)
                {
                    try
                    {
                        if (this.SimpleSSHClient.ReceiveText().Length > 0)
                        {
                            this.Disconnect();
                            this.Write("Client Close.", MessageType.INFO);
                        }
                    }
                    catch (Exception ex2)
                    {
                        flag = false;
                    }
                }
            }
            if (!this.SimpleSSHClient.Active)
            {
                this.timer_0.Enabled = false;
            }
        }
        private void Disconnect()
        {
            if (this.Connected)
            {
                this.SimpleSSHClient.Close();
                modCommon.sshClient.Disconnect();
                modCommon.sshClient.Dispose();
            }
            this.Connected = false;
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottom(rtbLogs);
        }

        private void method_12(object sender, EventArgs e)
        {
            bool flag = true;
            while (flag)
            {
                try
                {
                    flag = this.SimpleSSHClient.CanReceive(0);
                }
                catch (Exception ex)
                {
                    flag = false;
                }
                if (flag)
                {
                    try
                    {
                        int length = this.SimpleSSHClient.ReceiveText().Length;
                    }
                    catch (Exception ex2)
                    {
                        flag = false;
                    }
                }
            }
            if (!this.SimpleSSHClient.Active)
            {
                this.timer_1.Enabled = false;
            }

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
                        PingLabel.Text = "Desconectado";
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                    PingLabel.Text = "Desconectado";
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
            while (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

            if (this.FirmwareLabel.Text.Contains("XC"))
            {
                MessageBox.Show("El equipo no es de la Linea Ubiquiti airMAX M!.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Write("Error! Detected as Airmax AC device.", MessageType.ERROR);
                this.Write("XC firmware not supported.", MessageType.ERROR);
                return;
            }
            if (this.FirmwareLabel.Text.Contains("WA"))
            {
                MessageBox.Show("El equipo no es de la Linea Ubiquiti airMAX M!.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Write("Error! Detected as Airmax AC device.", MessageType.ERROR);
                this.Write("WA firmware not supported.", MessageType.ERROR);
                return;
            }

            this.Write("Intentando activar el Compliance Test al equipo", MessageType.INFO);
            //SshCommand sshCommand = modCommon.sshClient.RunCommand(modCommon.GetCTCode());
            this.SimpleSSHClient.SendText(modCommon.GetCTCode() + "\r\n");
            this.Write("El equipo se reiniciará", MessageType.INFO);
            Thread.Sleep(5000);
            this.timer_0.Enabled = true;

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

                            Thread.Sleep(2500);
                            if (pais == "COMPLIANCE TEST")
                            {
                                this.Write("Hecho... Compliance Test activado.", MessageType.INFO);
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
    }
}
