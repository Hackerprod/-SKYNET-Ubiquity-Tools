using System;
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;
using Renci.SshNet;
using SkynetChat.Controles;
using UbntDiscovery;

namespace SKYNET.GUI
{
    [ComVisibleAttribute(true)]
    public partial class frmSearch : frmBase
    {
        public StringBuilder HtmlString;
        public bool Searching;
        public BoxTool boxTool;
        public static frmSearch frm;

        public DeviceDiscovery DeviceDiscovery;

        public frmSearch()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;

            SetMouseMove(PN_Top);
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

        private void Method_Click(object sender, EventArgs e)
        {
            if (!Common.sshClient.IsConnected)
            {
                Common.sshClient.Connect();
            }
            SshCommand sshCommand = Common.sshClient.RunCommand("wstalist");
            string Result = sshCommand.Result;
            textBox1.Text += Result;
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
