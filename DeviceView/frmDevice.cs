using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DeviceView
{
    public partial class frmDevice : Form
    {
        List<ListViewItem> Frecuencys;
        frmMain fMain;
        public frmDevice(List<ListViewItem> frecuencys, frmMain main)
        {
            InitializeComponent();
            Frecuencys = frecuencys;
            fMain = main;
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            DeviceSignal BestSignal = new DeviceSignal() { Channel = 0, Average = -200 };

            Frecuencys.Sort((s1, s2) => s1.SubItems[3].Text.CompareTo(s2.SubItems[3].Text));

            foreach (ListViewItem item in Frecuencys)
            {
                _lvAliveHosts.Items.Add((ListViewItem)item.Clone());

                if (Convert.ToInt32(item.SubItems[3].Text) > BestSignal.Average)
                    BestSignal = new DeviceSignal() { Channel = Convert.ToInt32(item.SubItems[0].Text), Average = Convert.ToInt32(item.SubItems[3].Text) };
            }

            //SetBestSignal(BestSignal.Channel);
            label1.Text = "Best average signal, channel  " + BestSignal.Channel + ", average signal " + BestSignal.Average + " dbm";
        }

        private void _lvAliveHosts_MouseHover(object sender, EventArgs e)
        {
        }

        private void Devices1_MouseClick(object sender, MouseEventArgs e)
        {
            devices1.Visible = true;
            devices1.Location = new System.Drawing.Point(e.Location.X, e.Location.Y);

        }

        private void _lvAliveHosts_MouseClick(object sender, MouseEventArgs e)
        {
            string name = _lvAliveHosts.SelectedItems[0].SubItems[0].Text;
            List<ListViewItem> Frecuencys = GetItemsFromChannel(name);

            devices1.Visible = true;
            SetLocation();
            devices1.SetDevices(Frecuencys);
            try
            {


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

            foreach (ListViewItem item in fMain._lvAliveHosts.Items)
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

        private void FrmDevice_Resize(object sender, EventArgs e)
        {
            SetLocation();
        }

        private void Devices1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Devices1_MouseLeave(object sender, EventArgs e)
        {
            devices1.Visible = false;
        }
    }
}
