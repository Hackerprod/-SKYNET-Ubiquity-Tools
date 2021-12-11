using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using SKYNET.LOG;
using System.Timers;
using Renci.SshNet;
using UbntDiscovery;
using System.Web.Script.Serialization;

namespace SKYNET
{
    [ComVisibleAttribute(true)]
    public partial class frmDevice : Form
    {
        private bool mouseDown;     //Mover ventana
        private Point lastLocation; //Mover ventana
        private readonly Dictionary<string, string> UsersAndIds = new Dictionary<string, string>();
        public static frmDevice frm;
        private static ILog ilog_0;
        public StringBuilder HtmlString;
        public bool Searching;
        private SshCommand sshCommand;
        private int deviceDiscovereds;
        private int y = 7;

        public DeviceDiscovery DeviceDiscovery { get; private set; }


        List<ListViewItem> Frecuencys;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public frmDevice(List<ListViewItem> frecuencys)
        {
            InitializeComponent();
            frm = this;
            Frecuencys = frecuencys;

            CheckForIllegalCrossThreadCalls = false;

            ilog_0 = new ILog();
            //deviceDiscovereds = new List<DeviceDiscovered>();

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

            if (!modCommon.sshClient.IsConnected)
            {
                modCommon.sshClient.Connect();
            }

            sshCommand = modCommon.sshClient.RunCommand("wstalist");

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

            //30, 31, 34
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



        private void FrmDiscovery_Deactivate(object sender, EventArgs e)
        {
            //Close();
        }

        private void FrmConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void Devices1_MouseHover(object sender, EventArgs e)
        {
        }

        private void Devices1_MouseLeave(object sender, EventArgs e)
        {
            devices1.Visible = false;
        }

        private void Devices1_MouseMove(object sender, MouseEventArgs e)
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
