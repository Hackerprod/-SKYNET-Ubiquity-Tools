using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using SKYNET.LOG;
using Renci.SshNet;
using UbntDiscovery;
using System.Web.Script.Serialization;

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmConnection : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmConnection frm;
        private static ILog ilog_0;
        public StringBuilder HtmlString;
        public bool Searching;
        private SshCommand sshCommand;
        private int deviceDiscovereds;
        private int y = 7;

        public DeviceDiscovery DeviceDiscovery { get; private set; }

        private CancellationTokenSource _cts;
        private Task discoveryTask;
        private int colorNum;
        List<int> Channels;
        private string CurrentChannel;

        public frmConnection(List<int> channels, string currentChannel)
        {
            InitializeComponent();
            frm = this;
            Channels = channels;
            CurrentChannel = currentChannel;
            Channels.Sort((s1, s2) => s1.CompareTo(s2));

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
                    method1.Enabled = false;
                }
                else
                {
                    method1.Enabled = true;
                }
            }

        }


        private void FrmDiscovery_Deactivate(object sender, EventArgs e)
        {
            //Close();
        }


        private void Mothod_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Mothod_MouseLeave(object sender, EventArgs e)
        {

        }
        Thread StartSearch;
        private void Method_Click(object sender, EventArgs e)
        {
            Thread searhThread = new Thread(Go);
            searhThread.IsBackground = true;
            searhThread.Start();
        }

        private void Go()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            _lvAliveHosts.Items.Clear();
            label1.Text = "Búsqueda en progreso... tiempo estimado " + DurationToString(TimeSpan.FromMinutes(Channels.Count + 3)) + " minutes";


            List<DeviceSignal> Signals = new List<DeviceSignal>();

            SshCommand sshCommand;

            progressBar1.Visible = true;
            progressBar1.Maximum = Channels.Count;

            for (int i = 0; i < Channels.Count; i++)
            {
                progressBar1.Value = i;

                string Channel = Channels[i].ToString();

                if (!modCommon.sshClient.IsConnected)
                {
                    modCommon.sshClient.Connect();
                }
                if (!string.IsNullOrEmpty(Channels[i].ToString()))
                {
                    if (Channel == "0")
                    {
                        Channel = "00";
                    }
                    sshCommand = modCommon.sshClient.RunCommand("iwconfig ath0 channel " + Channel + "&& save");
                    Write("Cambiando al canal " + Channel);
                }

                if (!modCommon.sshClient.IsConnected)
                {
                    modCommon.sshClient.Connect();
                }

                Write("Esperando 60 segundos para comprobar calidad");
                Thread.Sleep(60000);

                Write("Chequeando calidad de las estaciones");
                sshCommand = modCommon.sshClient.RunCommand("wstalist");

                string Result = sshCommand.Result;

                if (Directory.Exists("Data"))
                {
                    File.WriteAllText("Data/Channel_" + Channel + ".json", Result);
                }

                List<DeviceClient> clients = new JavaScriptSerializer().Deserialize<List<DeviceClient>>(Result);

                int average = 0;
                int count = 0;

                if (clients.Any())
                {
                    foreach (DeviceClient client in clients)
                    {
                        AddClient(client, Channel);

                        try
                        {
                            average += client.remote.signal;
                            count++;
                        }
                        catch
                        { }
                    }
                    if (average != 0)
                    {
                        average = average / count;
                        Signals.Add(new DeviceSignal() { Channel = Channel, Average = average });
                    }

                }
            }

            DeviceSignal BestSignal = null;
            foreach (DeviceSignal item in Signals)
            {
                if (BestSignal == null)
                    BestSignal = item;

                if (item.Average > BestSignal.Average)
                    BestSignal = item;
            }

            SetBestSignal(BestSignal.Channel);
            label1.Text = "Best average signal, channel  " + BestSignal.Channel + ", average signal " + BestSignal.Average + " dbm";

            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }
            sshCommand = modCommon.sshClient.RunCommand("iwconfig ath0 channel " + CurrentChannel + "&& save");
            progressBar1.Visible = false;
        }

        private string DurationToString(TimeSpan duration)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (duration.Days > 0)
            {
                stringBuilder.Append(duration.Days);
                stringBuilder.Append((duration.Days > 1) ? " days " : " day ");
            }
            stringBuilder.AppendFormat("{0:d2} : {1:d2} : {2:d2}", duration.Hours, duration.Minutes, duration.Seconds);
            return stringBuilder.ToString();
        }
        private void SetBestSignal(string channel)
        {
            foreach (ListViewItem item in _lvAliveHosts.Items)
            {
                if (item.Tag == (object)channel)
                {
                    item.ForeColor = Color.Red;
                }
            }
        }
        private void Write(object v)
        {
            txt.Text += v.ToString() + Environment.NewLine;
        }

        private void AddClient(DeviceClient device, string channel)
        {
            Color backColor = GetBackClolor(channel);


            try
            {
                ListViewItem listViewItem = new ListViewItem();

                listViewItem.BackColor = backColor;

                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                listViewItem.SubItems.Add("");
                _lvAliveHosts.Items.Add(listViewItem);
                //_lvAliveHosts.Sort();

                string remote = "";
                if (device.remote != null)
                {
                    remote = device.remote.signal.ToString();
                }
                else
                    remote = device.signal.ToString();


                listViewItem.Tag = channel;
                listViewItem.SubItems[0].Text = channel;
                listViewItem.SubItems[1].Text = device.name;
                listViewItem.SubItems[2].Text = device.mac;
                listViewItem.SubItems[3].Text = remote + " / " + device.signal.ToString();
                listViewItem.SubItems[4].Text = device.lastip;
                listViewItem.SubItems[5].Text = device.tx.ToString();
                listViewItem.SubItems[6].Text = device.rx.ToString();
                listViewItem.SubItems[7].Text = device.ccq;
                listViewItem.SubItems[8].Text = device.txpower;
                listViewItem.SubItems[9].Text = device.noisefloor;
                listViewItem.SubItems[10].Text = device.tx_latency;
                listViewItem.SubItems[11].Text = device.signal.ToString();
            }
            catch (Exception)
            {
            }

            //30, 31, 34
        }

        string LastChannel;
        Color LastColor;
        Color Color1;
        Color Color2;

        private Color GetBackClolor(string channel)
        {
            Color1 = Color.FromArgb(42, 46, 51);
            Color2 = Color.FromArgb(30, 31, 34);

            Color back = Color.FromArgb(42, 46, 51);

            if (channel == LastChannel)
            {
                if (LastColor == Color1)
                    back = Color1;
                else
                    back = Color2;
            }
            else
            {
                LastChannel = channel;

                if (LastColor == Color1)
                    back = Color2;
                else
                    back = Color1;
                LastColor = back;
            }
            return back;
        }

        private void FrmConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (StartSearch != null)
            {
                if (!StartSearch.IsAlive)
                {
                    StartSearch.Abort();
                }
            }
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottom(txt);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;
        public static void ScrollToBottom(RichTextBox MyRichTextBox)
        {
            SendMessage(MyRichTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            _lvAliveHosts.Items.Clear();

            Write("Buscando salva de búsquedas anteriores.");

            List<DeviceSignal> Signals = new List<DeviceSignal>();
            List<DeviceFile> deviceSignals = new List<DeviceFile>();

            List<string> Directories = Directory.GetFiles(Environment.CurrentDirectory + "/Data", "*.json").ToList();

            foreach (var item in Directories)
            {
                string channel = "";
                channel = Path.GetFileNameWithoutExtension(item);
                channel = channel.Replace("Channel_", "");
                string json = File.ReadAllText(item);
                deviceSignals.Add(new DeviceFile() { Channel = Convert.ToInt32(channel), Json = json });
            }

            deviceSignals.Sort((s1, s2) => s1.Channel.CompareTo(s2.Channel));

            if (!deviceSignals.Any())
            {
                Write("No se han encontrado salvas en el programa.");
                return;
            }
            Write("Cargando archivos al programa.");

            foreach (DeviceFile item in deviceSignals)
            {
                string channel = item.Channel.ToString();
                string json = item.Json;
                List<DeviceClient> clients = new JavaScriptSerializer().Deserialize<List<DeviceClient>>(json);

                int average = 0;
                int count = 0;

                if (clients.Any())
                {
                    foreach (DeviceClient client in clients)
                    {
                        AddClient(client, channel);
                        try
                        {
                            average += client.remote.signal;
                            count++;
                        }
                        catch
                        {
                            average += client.signal;
                            count++;
                            //modCommon.Show(client.signal);
                        }
                    }
                }
                if (average != 0)
                {
                    average = average / count;
                    Signals.Add(new DeviceSignal() { Channel = channel, Average = average });
                }
            }

            DeviceSignal BestSignal = null;
            foreach (DeviceSignal item in Signals)
            {
                if (BestSignal == null)
                    BestSignal = item;

                if (item.Average > BestSignal.Average)
                    BestSignal = item;
            }
            SetBestSignal(BestSignal.Channel);
            label1.Text = "Best average signal, channel  " + BestSignal.Channel + ", average signal " + BestSignal.Average + " dbm";

        }


        private void _lvAliveHosts_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string name = _lvAliveHosts.SelectedItems[0].SubItems[1].Text;
                List<ListViewItem> Frecuencys = GetItemsFromName(name);
                frmDevice Manager = new frmDevice(Frecuencys);
                Manager.Show();
            }
            catch { }
        }
        private List<ListViewItem> GetItemsFromName(string name)
        {
            List<ListViewItem> Items = new List<ListViewItem>();

            foreach (ListViewItem item in _lvAliveHosts.Items)
            {
                if (item.SubItems[1].Text == name)
                {
                    ListViewItem Current = (ListViewItem)item.Clone();
                    Color2 = Color.FromArgb(30, 31, 34);

                    Current.BackColor = Color.FromArgb(42, 46, 51);
                    string[] power = item.SubItems[3].Text.Split(' ');
                    Current.SubItems[3].Text = power[0];
                    Items.Add(Current);
                }
            }
            return Items;
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
