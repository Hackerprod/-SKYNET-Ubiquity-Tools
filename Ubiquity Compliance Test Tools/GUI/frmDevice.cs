using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Timers;
using Renci.SshNet;
using System.Web.Script.Serialization;
using SKYNET.Models;

namespace SKYNET.GUI
{
    public partial class frmDevice : frmBase
    {
        public static frmDevice frm;
        public bool Searching;
        public StringBuilder HtmlString;

        private SshCommand sshCommand;
        private List<ListViewItem> Frecuencys;
        private System.Timers.Timer _timer;

        public frmDevice(List<ListViewItem> frecuencys)
        {
            InitializeComponent();
            frm = this;
            Frecuencys = frecuencys;
            SetMouseMove(PN_Top);

            CheckForIllegalCrossThreadCalls = false;

            _timer = new System.Timers.Timer();

            DeviceSignal BestSignal = new DeviceSignal() { Channel = "0", Average = -200 };

            Frecuencys.Sort((s1, s2) => s1.SubItems[3].Text.CompareTo(s2.SubItems[3].Text));

            foreach (ListViewItem item in Frecuencys)
            {
                _lvAliveHosts.Items.Add((ListViewItem)item.Clone());

                if (Convert.ToInt32(item.SubItems[3].Text) > BestSignal.Average)
                    BestSignal = new DeviceSignal() { Channel = item.SubItems[0].Text, Average = Convert.ToInt32(item.SubItems[3].Text) };
            }

            //SetBestSignal(BestSignal.Channel);
            label1.Text = "Best signal, channel  " + BestSignal.Channel + ", signal " + BestSignal.Average + " dbm";
        }

        public frmDevice()
        {
            InitializeComponent();
            frm = this;

            label1.Text = "Monitoreando estaciones conectadas al equipo.";
            tittleLbl.Text = "Conexiones entre AP - Estaciones";

            Chann.Width = 0;
            InitTimer();
        }

        private void InitTimer()
        {
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;

            _timer.Interval = (double)1000;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }

            sshCommand = Common.sshClient.RunCommand("wstalist");

            string Result = sshCommand.Result;

            List<DeviceClient> clients = new JavaScriptSerializer().Deserialize<List<DeviceClient>>(Result);

            if (clients.Count == _lvAliveHosts.Items.Count)
            {
                foreach (DeviceClient client in clients)
                {
                    UpdateDevices(client, "");
                }

            }
            else
            {
                _lvAliveHosts.Items.Clear();

                if (clients.Any())
                {
                    foreach (DeviceClient client in clients)
                    {
                        AddClient(client, "");
                    }
                }
            }

            _timer.Interval = (double)1000;
            _timer.Start();
        }

        private void AddClient(DeviceClient device, string channel)
        {
            try
            {
                ListViewItem listViewItem = new ListViewItem();

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

        private void UpdateDevices(DeviceClient device, string channel)
        {
            try
            {
                foreach (ListViewItem item in _lvAliveHosts.Items)
                {
                    if (item.SubItems[2].Text == device.mac)
                    {
                        string remote = "";
                        if (device.remote != null)
                        {
                            remote = device.remote.signal.ToString();
                        }
                        else
                            remote = device.signal.ToString();

                        item.SubItems[1].Text = device.name;
                        item.SubItems[3].Text = remote + " / " + device.signal.ToString();
                        item.SubItems[4].Text = device.lastip;
                        item.SubItems[5].Text = device.tx.ToString();
                        item.SubItems[6].Text = device.rx.ToString();
                        item.SubItems[7].Text = device.ccq;
                        item.SubItems[8].Text = device.txpower;
                        item.SubItems[9].Text = device.noisefloor;
                        item.SubItems[10].Text = device.tx_latency;
                        item.SubItems[11].Text = device.signal.ToString();

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void FrmDiscovery_Deactivate(object sender, EventArgs e)
        {
            //Close();
        }

        private void _lvAliveHosts_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string name = _lvAliveHosts.SelectedItems[0].SubItems[0].Text;
                List<ListViewItem> Frecuencys = GetItemsFromChannel(name);

                SetLocation();
                devices1.SetDevices(Frecuencys);
                devices1.Visible = true;
            }
            catch { }
        }

        private void SetLocation()
        {
            int X = (this.Width - devices1.Width) - 45;
            int Y = (this.Height - devices1.Height) - 60;
            devices1.Location = new Point(X, Y);
        }

        private List<ListViewItem> GetItemsFromChannel(string channel)
        {
            List<ListViewItem> Items = new List<ListViewItem>();

            foreach (ListViewItem item in frmConnection.frm._lvAliveHosts.Items)
            {
                if (item.SubItems[0].Text == channel)
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
