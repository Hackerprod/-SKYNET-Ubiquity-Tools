using System;
using SKYNET.GUI.Controls;

namespace SKYNET.GUI
{
    public partial class frmComplianceTest : frmBase
    {
        public static frmComplianceTest frm;

        public frmComplianceTest()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;
            SetMouseMove(PN_Top);
        }

        private void Method_Click(object sender, EventArgs e)
        {
            if (sender is SKYNET_Button)
            {
                SKYNET_Button button = (SKYNET_Button)sender;
                if (button.Name == "BT_International")
                {
                    frmMain.CT_Method = CT_Method.International;
                }
                else if (button.Name == "BT_USA")
                {
                    frmMain.CT_Method = CT_Method.USA;
                }
                Close();
            }
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
