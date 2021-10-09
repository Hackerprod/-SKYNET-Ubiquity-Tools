using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceView
{
    public partial class Devices : UserControl
    {
        private string _average;

        public Devices()
        {
            InitializeComponent();
        }
        public string Average
        {
            get { return _average; }
            set
            {
                _average = value;
                Info.Text = "Average: " + value;
            }
        }
        private void Info_Click(object sender, EventArgs e)
        {

        }
        int average = 0;
        int count = 0;
        internal void SetDevices(List<ListViewItem> frecuencys)
        {
            _lvAliveHosts.Items.Clear();
            average = 0;
            count = 0;

            foreach (ListViewItem item in frecuencys)
            {
                AddDevice(item);
            }
            try
            {
                Average = (average / count).ToString();
            }
            catch  { }
        }

        private void AddDevice(ListViewItem item)
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
            _lvAliveHosts.Items.Add(listViewItem);



            listViewItem.SubItems[0].Text = item.SubItems[0].Text;
            listViewItem.SubItems[1].Text = item.SubItems[1].Text;
            listViewItem.SubItems[2].Text = item.SubItems[3].Text;
            listViewItem.SubItems[3].Text = item.SubItems[4].Text;
            listViewItem.SubItems[4].Text = item.SubItems[5].Text;
            listViewItem.SubItems[5].Text = item.SubItems[6].Text;
            listViewItem.SubItems[6].Text = item.SubItems[7].Text;
            listViewItem.SubItems[7].Text = item.SubItems[9].Text;
            listViewItem.SubItems[8].Text = item.SubItems[15].Text;

            count++;
            average += Convert.ToInt32(item.SubItems[3].Text);
        }

        private void This_MouseHover(object sender, EventArgs e)
        {
            base.OnMouseHover(e);
        }
        private void This_MouseLeave(object sender, EventArgs e)
        {
            base.OnMouseLeave(e);
        }
        private void This_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
    }
}
