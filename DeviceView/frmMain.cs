using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;

namespace DeviceView
{
    public partial class frmMain : Form
    {
        public static frmMain frm;
        public frmMain()
        {
            InitializeComponent();
            frm = this;
        }

        private void Do_Click(object sender, EventArgs e)
        {
            //List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(modCommon.GetClientCode());
            List<DeviceClient> clients = new JavaScriptSerializer().Deserialize<List<DeviceClient>>(modCommon.GetClientCode());

            foreach (DeviceClient client in clients)
            {
                AddClient(client, "2");
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            List<DeviceSignal> Signals = new List<DeviceSignal>();
            List<DeviceFile> deviceSignals = new List<DeviceFile>();

            List<string> Directories = Directory.GetFiles(Environment.CurrentDirectory, "*.json").ToList();

            foreach (var item in Directories)
            {
                string channel = "";
                channel = Path.GetFileNameWithoutExtension(item);
                channel = channel.Replace("Channel_", "");
                string json = File.ReadAllText(item);
                deviceSignals.Add(new DeviceFile() { Channel = Convert.ToInt32(channel), Json = json });
            }

            deviceSignals.Sort((s1, s2) => s1.Channel.CompareTo(s2.Channel));

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
                    Signals.Add(new DeviceSignal() { Channel = Convert.ToInt32(channel), Average = average });
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

        private void SetBestSignal(int channel)
        {
            foreach (ListViewItem item in _lvAliveHosts.Items)
            {
                if (item.Tag.ToString() == channel.ToString())
                {
                    item.ForeColor = Color.Red;
                }
            }
        }

        string LastChannel;
        Color LastColor;
        Color Color1;
        Color Color2;

        private void AddClient(DeviceClient device, string channel)
        {
            Color1 = Color.White;
            Color2 = Color.Gainsboro; 

            Color BackColor = (Color)default;

            if (channel == LastChannel)
            {
                if (LastColor == Color1)
                {
                    BackColor = Color1;
                }
                else
                {
                    BackColor = Color2;
                }
            }
            else
            {
                LastChannel = channel;

                if (LastColor == Color1)
                {
                    BackColor = Color2;
                }
                else
                {
                    BackColor = Color1;
                }
                LastColor = BackColor;
            }


            ListViewItem listViewItem = new ListViewItem();
            listViewItem.BackColor = BackColor;
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

            string local = "";
            if (device.remote != null)
                local = device.remote.signal.ToString();
            else
                local = device.signal.ToString();

            listViewItem.Tag = channel;
            listViewItem.SubItems[0].Text = channel;
            listViewItem.SubItems[1].Text = device.name;
            listViewItem.SubItems[2].Text = device.mac;
            listViewItem.SubItems[3].Text = local + " / " + device.signal.ToString();
            listViewItem.SubItems[4].Text = device.lastip;
            listViewItem.SubItems[5].Text = device.tx.ToString();
            listViewItem.SubItems[6].Text = device.rx.ToString();
            listViewItem.SubItems[7].Text = device.ccq;
            listViewItem.SubItems[8].Text = device.txpower;
            listViewItem.SubItems[9].Text = device.noisefloor;
            listViewItem.SubItems[10].Text = device.associd;
            listViewItem.SubItems[11].Text = device.aprepeater;
            listViewItem.SubItems[12].Text = device.rssi;
            listViewItem.SubItems[13].Text = device.rx_chainmask;
            listViewItem.SubItems[14].Text = device.idle;
            listViewItem.SubItems[15].Text = device.tx_latency;
            listViewItem.SubItems[16].Text = device.uptime;
            listViewItem.SubItems[17].Text = device.ack;
            listViewItem.SubItems[18].Text = device.distance;
            listViewItem.SubItems[19].Text = device.signal.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {


        }

        private void _lvAliveHosts_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string MAC = _lvAliveHosts.SelectedItems[0].SubItems[2].Text;
                List<ListViewItem> Frecuencys = GetItemsFromName(MAC);
                frmDevice Manager = new frmDevice(Frecuencys, this);
                Manager.Show();
            }
            catch { }

        }

        private List<ListViewItem> GetItemsFromName(string mac)
        {
            List<ListViewItem> Items = new List<ListViewItem>();

            foreach (ListViewItem item in _lvAliveHosts.Items)
            {
                if (item.SubItems[2].Text == mac)
                {
                    ListViewItem Current = (ListViewItem)item.Clone();
                    Current.BackColor = Color.White;
                    string[] power = item.SubItems[3].Text.Split(' ');
                    Current.SubItems[3].Text = power[0];
                    Items.Add(Current);
                }
            }
            return Items;
        }
    }
}
