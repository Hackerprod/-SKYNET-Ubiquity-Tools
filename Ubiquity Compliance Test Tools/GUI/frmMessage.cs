using System;

namespace SKYNET.GUI
{
    public partial class frmMessage : frmBase
    {
        public TypeMessage typeMessage;

        public frmMessage(string message, TypeMessage type)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;  
            SetMouseMove(PN_Top);

            typeMessage = type;

            switch (typeMessage)
            {
                case TypeMessage.Alert:

                    break;
                case TypeMessage.Normal:
                    acepctBtn.Visible = false;
                    cancelBtn.Text = "Cerrar";
                    break;
                case TypeMessage.YesNo:

                    break;
            }
            txtMessage.Text = message;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Cancel.PerformClick();
            Close();
        }

        private void acepctBtn_Click(object sender, EventArgs e)
        {
            ok.PerformClick();
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

