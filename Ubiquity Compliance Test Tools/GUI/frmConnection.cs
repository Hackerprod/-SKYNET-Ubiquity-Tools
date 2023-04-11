using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using Renci.SshNet;
using System.Web.Script.Serialization;
using SKYNET.Models;

namespace SKYNET.GUI
{
    public partial class frmConnection : frmBase
    {
        public static frmConnection frm;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        private int colorNum;
        private List<int> Channels;
        private string CurrentChannel;

        private string LastChannel;
        private Color LastColor;
        private Color Color1;
        private Color Color2;

        public frmConnection(List<int> channels, string currentChannel)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            frm = this;
            Channels = channels;
            CurrentChannel = currentChannel;
            Channels.Sort((s1, s2) => s1.CompareTo(s2));
            SetMouseMove(PN_Top);
        }

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

                if (!Common.sshClient.IsConnected)
                {
                    Common.sshClient.Connect();
                }
                if (!string.IsNullOrEmpty(Channels[i].ToString()))
                {
                    if (Channel == "0")
                    {
                        Channel = "00";
                    }
                    sshCommand = Common.sshClient.RunCommand("iwconfig ath0 channel " + Channel + "&& save");
                    Write("Cambiando al canal " + Channel);
                }

                if (!Common.sshClient.IsConnected)
                {
                    Common.sshClient.Connect();
                }

                Write("Esperando 60 segundos para comprobar calidad");
                Thread.Sleep(60000);

                Write("Chequeando calidad de las estaciones");
                sshCommand = Common.sshClient.RunCommand("wstalist");

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

            if (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }
            sshCommand = Common.sshClient.RunCommand("iwconfig ath0 channel " + CurrentChannel + "&& save");
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

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            ScrollToBottom(txt);
        }

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
                            //Common.Show(client.signal);
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
