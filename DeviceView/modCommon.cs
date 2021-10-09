using System;
using System.IO;
using System.Windows.Forms;

namespace DeviceView
{
    internal class modCommon
    {
        internal static string GetClientCode()
        {
            return File.ReadAllText("Code.txt");
        }
        public static void Show(object mess)
        {
            MessageBox.Show(mess.ToString());
        }
    }
}