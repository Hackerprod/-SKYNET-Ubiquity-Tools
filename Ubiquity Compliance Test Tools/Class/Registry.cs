using Microsoft.Win32;
using System;
using System.Windows.Forms;


namespace SKYNET
{
    public class RegistrySettings
    {
        public static RegistryKey UbiquityTools { get; private set; }

        public static void Initialice()
        {
            UbiquityTools = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\[SKYNET] Ubiquity Tools\", true);
            if (UbiquityTools == null)
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\[SKYNET] Ubiquity Tools\");
                UbiquityTools = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\[SKYNET] Ubiquity Tools\", true);
            }
        }

        public static void SaveSettings()
        {
            try { UbiquityTools.SetValue("server", frmMain.frm.serverip.Text); } catch {  }
            try { UbiquityTools.SetValue("username", frmMain.frm.username.Text); } catch {  }
            try { UbiquityTools.SetValue("password", frmMain.frm.password.Text); } catch {  }
        }
        public static void LoadSettings()
        {

            try { frmMain.frm.serverip.Text = (string)UbiquityTools.GetValue("server", RegistryValueKind.String); } catch { }
            try { frmMain.frm.username.Text = (string)UbiquityTools.GetValue("username", RegistryValueKind.String); } catch { }
            try { frmMain.frm.password.Text = (string)UbiquityTools.GetValue("password", RegistryValueKind.String); } catch { }
        }

    }
}
