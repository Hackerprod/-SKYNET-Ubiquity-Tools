using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using Renci.SshNet;
using SKYNET.LOG;
using SKYNET.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace SKYNET
{
    internal class modCommon
    {

        private static readonly ILog ilog = new ILog("MODCOMMON");
        public static SshClient sshClient;
        

        static modCommon()
        {
            sshClient = new SshClient(new PasswordConnectionInfo(frmMain.frm.serverip.Text, 22, frmMain.frm.username.Text, frmMain.frm.password.Text));
        }


        public static void Log(object msg, MessageType type)
        {
            switch (type)
            {
                case MessageType.INFO: ilog.Info(msg.ToString()); break;
                case MessageType.WARN: ilog.Warn(msg.ToString()); break;
                case MessageType.ERROR: ilog.Error(msg.ToString()); break;
                case MessageType.DEBUG: ilog.Debug(msg.ToString()); break;
            }
        }
        public static void Write(object msg, MessageType type)
        {
            switch (type)
            {
                case MessageType.INFO: ilog.Info(msg.ToString()); break;
                case MessageType.WARN: ilog.Warn(msg.ToString()); break;
                case MessageType.ERROR: ilog.Error(msg.ToString()); break;
                case MessageType.DEBUG: ilog.Debug(msg.ToString()); break;
            }
        }
        public static ILog log = new ILog();
        internal static Device device;

        public static bool Hackerprod
        {
            get
            {
                if (Environment.UserName == "Hackerprod" || Environment.MachineName == "Musicalprod")
                    return true;
                else return false;
            }
            internal set { }
        }

        public static void Show(object msg)
        {
            frmMessage message = new frmMessage(msg.ToString(), TypeMessage.Normal);
            message.Show();
        }
        public static string GetExePatch()
        {
            Process currentProcess;
            try
            {
                currentProcess = Process.GetCurrentProcess();
                return new FileInfo(currentProcess.MainModule.FileName).FullName;
            }
            finally { currentProcess = null; }
        }
        public static string GetUniqueAlphaNumericID()
        {
            string str1 = "";
            try
            {
                //Random Marcado por mi

                string str2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                short num1 = checked((short)str2.Length);
                Random random = new Random();
                StringBuilder stringBuilder = new StringBuilder();
                int num2 = 1;
                do
                {
                    int startIndex = random.Next(0, (int)num1);
                    stringBuilder.Append(str2.Substring(startIndex, 1));
                    checked { ++num2; }
                }
                while (num2 <= 6);
                stringBuilder.Append(DateAndTime.Now.ToString("HHmmss"));
                str1 = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            return str1;
        }

        internal static string Firmware(string firmware)
        {
            string result = "";

            string[] part = firmware.Split('.');
            for (int i = 0; i < part.Length; i++)
            {
                try
                {
                    if (i == 0)
                        result += part[i] + ".";
                    else if (i == 2)
                        result += part[i] + ".";
                    else if (i == 3)
                        result += part[i] + ".";
                    else if (i == 4)
                        result += part[i];
                }
                catch { }
            }

            return result;
        }

        public static int RandomID()
        {
            Random r = new Random();
            return r.Next(1010, 9999);
        }

        public static string GetPatch()
        {
            Process currentProcess;
            try
            {
                currentProcess = Process.GetCurrentProcess();
                return new FileInfo(currentProcess.MainModule.FileName).Directory?.FullName;
            }
            finally { currentProcess = null; }
        }
        public static bool ConvertToBool(string boolean)
        {
            if (boolean.ToLower() == "false")
                return false;
            else return true;
        }

        public static byte[] GetBytes(object Object)
        {
            BinaryFormatter binary = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            binary.Serialize(stream, Object);
            return stream.ToArray();
        }
        public static object GetObject(byte[] loL)
        {
            BinaryFormatter binary = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(loL);
            var objeto = binary.Deserialize(stream);

            return objeto;
        }
        public static string Win32Path()
        {
            string correctGameFolder = "";
            RegistryKey exeKey = Registry.ClassesRoot.OpenSubKey("dota2\\Shell\\Open\\Command", true);
            if (exeKey != null)
            {
                string exeFile = exeKey.GetValue(null).ToString().Split('"')[1];
                correctGameFolder = Directory.GetParent(exeFile).ToString();

            }
            return correctGameFolder;
        }

        internal static string GetCountry(string left)
        {
            if (Operators.CompareString(left, Conversions.ToString(0), false) == 0)
            {
                return "?";
            }
            else if (Operators.CompareString(left, Conversions.ToString(32), false) == 0)
            {
                return "Argentina";
            }
            else if (Operators.CompareString(left, Conversions.ToString(51), false) == 0)
            {
                return "Armenia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(533), false) == 0)
            {
                return "Aruba";
            }
            else if (Operators.CompareString(left, Conversions.ToString(36), false) == 0)
            {
                return "Australia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(40), false) == 0)
            {
                return "Austria";
            }
            else if (Operators.CompareString(left, Conversions.ToString(31), false) == 0)
            {
                return "Azerbaijan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(48), false) == 0)
            {
                return "Bahrain";
            }
            else if (Operators.CompareString(left, Conversions.ToString(52), false) == 0)
            {
                return "Barbados";
            }
            else if (Operators.CompareString(left, Conversions.ToString(112), false) == 0)
            {
                return "Belarus";
            }
            else if (Operators.CompareString(left, Conversions.ToString(56), false) == 0)
            {
                return "Belgium";
            }
            else if (Operators.CompareString(left, Conversions.ToString(84), false) == 0)
            {
                return "Belize";
            }
            else if (Operators.CompareString(left, Conversions.ToString(68), false) == 0)
            {
                return "Bolivia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(70), false) == 0)
            {
                return "Bosnia and Herzegovina";
            }
            else if (Operators.CompareString(left, Conversions.ToString(76), false) == 0)
            {
                return "Brazil";
            }
            else if (Operators.CompareString(left, Conversions.ToString(96), false) == 0)
            {
                return "Brunei Darussalam";
            }
            else if (Operators.CompareString(left, Conversions.ToString(100), false) == 0)
            {
                return "Bulgaria";
            }
            else if (Operators.CompareString(left, Conversions.ToString(116), false) == 0)
            {
                return "Cambodia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(124), false) == 0)
            {
                return "Canada";
            }
            else if (Operators.CompareString(left, Conversions.ToString(152), false) == 0)
            {
                return "Chile";
            }
            else if (Operators.CompareString(left, Conversions.ToString(156), false) == 0)
            {
                return "China";
            }
            else if (Operators.CompareString(left, Conversions.ToString(170), false) == 0)
            {
                return "Colombia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(188), false) == 0)
            {
                return "Costa Rica";
            }
            else if (Operators.CompareString(left, Conversions.ToString(191), false) == 0)
            {
                return "Croatia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(196), false) == 0)
            {
                return "Cyprus";
            }
            else if (Operators.CompareString(left, Conversions.ToString(203), false) == 0)
            {
                return "Czech Republic";
            }
            else if (Operators.CompareString(left, Conversions.ToString(208), false) == 0)
            {
                return "Denmark";
            }
            else if (Operators.CompareString(left, Conversions.ToString(214), false) == 0)
            {
                return "Dominican Republic";
            }
            else if (Operators.CompareString(left, Conversions.ToString(218), false) == 0)
            {
                return "Ecuador";
            }
            else if (Operators.CompareString(left, Conversions.ToString(818), false) == 0)
            {
                return "Egypt";
            }
            else if (Operators.CompareString(left, Conversions.ToString(222), false) == 0)
            {
                return "El Salvador";
            }
            else if (Operators.CompareString(left, Conversions.ToString(233), false) == 0)
            {
                return "Estonia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(246), false) == 0)
            {
                return "Finland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(250), false) == 0)
            {
                return "France";
            }
            else if (Operators.CompareString(left, Conversions.ToString(268), false) == 0)
            {
                return "Georgia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(276), false) == 0)
            {
                return "Germany";
            }
            else if (Operators.CompareString(left, Conversions.ToString(300), false) == 0)
            {
                return "Greece";
            }
            else if (Operators.CompareString(left, Conversions.ToString(304), false) == 0)
            {
                return "Greenland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(308), false) == 0)
            {
                return "Grenada";
            }
            else if (Operators.CompareString(left, Conversions.ToString(316), false) == 0)
            {
                return "Guam";
            }
            else if (Operators.CompareString(left, Conversions.ToString(320), false) == 0)
            {
                return "Guatemala";
            }
            else if (Operators.CompareString(left, Conversions.ToString(332), false) == 0)
            {
                return "Haiti";
            }
            else if (Operators.CompareString(left, Conversions.ToString(340), false) == 0)
            {
                return "Honduras";
            }
            else if (Operators.CompareString(left, Conversions.ToString(344), false) == 0)
            {
                return "Hong Kong";
            }
            else if (Operators.CompareString(left, Conversions.ToString(348), false) == 0)
            {
                return "Hungary";
            }
            else if (Operators.CompareString(left, Conversions.ToString(352), false) == 0)
            {
                return "Iceland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(356), false) == 0)
            {
                return "India";
            }
            else if (Operators.CompareString(left, Conversions.ToString(360), false) == 0)
            {
                return "Indonesia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(368), false) == 0)
            {
                return "Iraq";
            }
            else if (Operators.CompareString(left, Conversions.ToString(372), false) == 0)
            {
                return "Ireland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(376), false) == 0)
            {
                return "Israel";
            }
            else if (Operators.CompareString(left, Conversions.ToString(380), false) == 0)
            {
                return "Italy";
            }
            else if (Operators.CompareString(left, Conversions.ToString(388), false) == 0)
            {
                return "Jamaica";
            }
            else if (Operators.CompareString(left, Conversions.ToString(400), false) == 0)
            {
                return "Jordan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(398), false) == 0)
            {
                return "Kazakhstan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(404), false) == 0)
            {
                return "Kenya";
            }
            else if (Operators.CompareString(left, Conversions.ToString(410), false) == 0)
            {
                return "Korea Republic";
            }
            else if (Operators.CompareString(left, Conversions.ToString(414), false) == 0)
            {
                return "Kuwait";
            }
            else if (Operators.CompareString(left, Conversions.ToString(428), false) == 0)
            {
                return "Latvia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(422), false) == 0)
            {
                return "Lebanon";
            }
            else if (Operators.CompareString(left, Conversions.ToString(438), false) == 0)
            {
                return "Liechtenstein";
            }
            else if (Operators.CompareString(left, Conversions.ToString(440), false) == 0)
            {
                return "Lithuania";
            }
            else if (Operators.CompareString(left, Conversions.ToString(442), false) == 0)
            {
                return "Luxembourg";
            }
            else if (Operators.CompareString(left, Conversions.ToString(446), false) == 0)
            {
                return "Macau";
            }
            else if (Operators.CompareString(left, Conversions.ToString(807), false) == 0)
            {
                return "Macedonia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(458), false) == 0)
            {
                return "Malaysia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(470), false) == 0)
            {
                return "Malta";
            }
            else if (Operators.CompareString(left, Conversions.ToString(484), false) == 0)
            {
                return "Mexico";
            }
            else if (Operators.CompareString(left, Conversions.ToString(492), false) == 0)
            {
                return "Monaco";
            }
            else if (Operators.CompareString(left, Conversions.ToString(499), false) == 0)
            {
                return "Montenegro";
            }
            else if (Operators.CompareString(left, Conversions.ToString(504), false) == 0)
            {
                return "Morocco";
            }
            else if (Operators.CompareString(left, Conversions.ToString(524), false) == 0)
            {
                return "Nepal";
            }
            else if (Operators.CompareString(left, Conversions.ToString(528), false) == 0)
            {
                return "Netherlands";
            }
            else if (Operators.CompareString(left, Conversions.ToString(530), false) == 0)
            {
                return "Netherlands Antilles";
            }
            else if (Operators.CompareString(left, Conversions.ToString(554), false) == 0)
            {
                return "New Zealand";
            }
            else if (Operators.CompareString(left, Conversions.ToString(566), false) == 0)
            {
                return "Nigeria";
            }
            else if (Operators.CompareString(left, Conversions.ToString(578), false) == 0)
            {
                return "Norway";
            }
            else if (Operators.CompareString(left, Conversions.ToString(512), false) == 0)
            {
                return "Oman";
            }
            else if (Operators.CompareString(left, Conversions.ToString(586), false) == 0)
            {
                return "Pakistan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(591), false) == 0)
            {
                return "Panama";
            }
            else if (Operators.CompareString(left, Conversions.ToString(598), false) == 0)
            {
                return "Papua New Guinea";
            }
            else if (Operators.CompareString(left, Conversions.ToString(604), false) == 0)
            {
                return "Peru";
            }
            else if (Operators.CompareString(left, Conversions.ToString(608), false) == 0)
            {
                return "Philippines";
            }
            else if (Operators.CompareString(left, Conversions.ToString(616), false) == 0)
            {
                return "Poland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(620), false) == 0)
            {
                return "Portugal";
            }
            else if (Operators.CompareString(left, Conversions.ToString(630), false) == 0)
            {
                return "Puerto Rico (U.S. territory)";
            }
            else if (Operators.CompareString(left, Conversions.ToString(634), false) == 0)
            {
                return "Qatar";
            }
            else if (Operators.CompareString(left, Conversions.ToString(642), false) == 0)
            {
                return "Romania";
            }
            else if (Operators.CompareString(left, Conversions.ToString(643), false) == 0)
            {
                return "Russia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(646), false) == 0)
            {
                return "Rwanda";
            }
            else if (Operators.CompareString(left, Conversions.ToString(652), false) == 0)
            {
                return "Saint Barthelemy";
            }
            else if (Operators.CompareString(left, Conversions.ToString(682), false) == 0)
            {
                return "Saudi Arabia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(688), false) == 0)
            {
                return "Serbia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(702), false) == 0)
            {
                return "Singapore";
            }
            else if (Operators.CompareString(left, Conversions.ToString(703), false) == 0)
            {
                return "Slovakia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(705), false) == 0)
            {
                return "Slovenia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(710), false) == 0)
            {
                return "South Africa";
            }
            else if (Operators.CompareString(left, Conversions.ToString(724), false) == 0)
            {
                return "Spain";
            }
            else if (Operators.CompareString(left, Conversions.ToString(144), false) == 0)
            {
                return "Sri Lanka";
            }
            else if (Operators.CompareString(left, Conversions.ToString(752), false) == 0)
            {
                return "Sweden";
            }
            else if (Operators.CompareString(left, Conversions.ToString(756), false) == 0)
            {
                return "Switzerland";
            }
            else if (Operators.CompareString(left, Conversions.ToString(158), false) == 0)
            {
                return "Taiwan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(764), false) == 0)
            {
                return "Thailand";
            }
            else if (Operators.CompareString(left, Conversions.ToString(780), false) == 0)
            {
                return "Trinidad and Tobago";
            }
            else if (Operators.CompareString(left, Conversions.ToString(788), false) == 0)
            {
                return "Tunisia";
            }
            else if (Operators.CompareString(left, Conversions.ToString(792), false) == 0)
            {
                return "Turkey";
            }
            else if (Operators.CompareString(left, Conversions.ToString(804), false) == 0)
            {
                return "Ukraine";
            }
            else if (Operators.CompareString(left, Conversions.ToString(784), false) == 0)
            {
                return "United Arab Emirates";
            }
            else if (Operators.CompareString(left, Conversions.ToString(826), false) == 0)
            {
                return "United Kingdom";
            }
            else if (Operators.CompareString(left, Conversions.ToString(840), false) == 0)
            {
                return "United States";
            }
            else if (Operators.CompareString(left, Conversions.ToString(858), false) == 0)
            {
                return "Uruguay";
            }
            else if (Operators.CompareString(left, Conversions.ToString(860), false) == 0)
            {
                return "Uzbekistan";
            }
            else if (Operators.CompareString(left, Conversions.ToString(862), false) == 0)
            {
                return "Venezuela";
            }
            else if (Operators.CompareString(left, Conversions.ToString(704), false) == 0)
            {
                return "Viet Nam";
            }
            else if (Operators.CompareString(left, Conversions.ToString(511), false) == 0)
            {
                return "COMPLIANCE TEST";
            }
            else if (Operators.CompareString(left, Conversions.ToString(5000), false) == 0)
            {
                return "COMPLIANCE TEST";
            }
            else
            {
                return "?";
            }

        }

        internal static Image GetDeviceImage(string text)
        {
            if (text.ToLower().Contains("nano") && text.ToLower().Contains("loco"))
            {
                return Resources.loco_m2;
            }
            else if (text.ToLower().Contains("nano"))
            {
                return Resources.m2;
            }
            else if (text.ToLower().Contains("bullet"))
            {
                return Resources.bullet;
            }
            else
                return Resources.ic_launcher;

        }

        public static void SetVisibleControl(Control connect, bool visible)
        {
            connect.Visible = visible;
        }
        internal static string GetCTCode_M1()
        {
            return "touch /etc/persistent/ct && sed -i 's/radio.1.countrycode.*=*./radio.1.countrycode=511/g' /tmp/system.cfg && sed -i 's/radio.countrycode.*=*./radio.countrycode=511/g' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save && reboot";
        }
        internal static string GetCTCode_M2()
        {
            return "touch /etc/persistent/ct && sed -i 's/radio.1.countrycode.*=*./radio.1.countrycode=511/g' /tmp/system.cfg && sed -i 's/radio.countrycode.*=*./radio.countrycode=511/g' /tmp/system.cfg && echo '<option value='511' > Compliance Test</option>' >> /var/etc/ccodes.inc && echo \"countrycode = 511\" > /var/etc/atheros.conf && sed -i 's/840/511/g' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save && reboot";
        }
        internal static string GetActivateListChannelsCode()
        {
            string Command = "";
            //Activar Lista de canales
            Command += "sed -i '/wireless.1.scan_list.status/c\\wireless.1.scan_list.status=enabled' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save && ";

            //Save and reboot
            Command += "save";

            return Command;

        }
        internal static string GetListChannelsCode()
        {
            string Command = "";

            //Agregar todos los canales
            if (frmMain.frm.DeviceLabel.Text.Contains(" M2"))
            {
                Command += "sed -i '/wireless.1.scan_list.channels/c\\wireless.1.scan_list.channels=2312,2317,2322,2327,2332,2337,2342,2347,2352,2357,2362,2367,2372,2377,2382,2387,2392,2397,2402,2407,2412,2417,2422,2427,2432,2437,2442,2447,2452,2457,2462,2467,2472,2477,2482,2487,2492,2497,2502,2507,2512,2517,2522,2527,2532,2537,2542,2547,2552,2557,2562,2567,2572,2577,2582,2587,2592,2597,2602,2607,2612,2617,2622,2627,2632,2637,2642,2647,2652,2657,2662,2667,2672,2677,2682,2687,2692,2697,2702,2707,2712,2717,2722,2727,2732' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save && ";
            }
            else if (frmMain.frm.DeviceLabel.Text.Contains(" M5"))
            {
                Command += "sed -i '/wireless.1.scan_list.channels/c\\wireless.1.scan_list.channels=5150,5155,5160,5165,5170,5175,5180,5185,5190,5195,5200,5205,5210,5215,5220,5225,5230,5235,5240,5245,5250,5255,5260,5265,5270,5275,5280,5285,5290,5295,5300,5305,5310,5315,5320,5325,5330,5335,5340,5345,5350,5355,5360,5365,5370,5375,5380,5385,5390,5395,5400,5405,5410,5415,5420,5425,5430,5435,5440,5445,5450,5455,5460,5465,5470,5475,5480,5485,5490,5495,5500,5505,5510,5515,5520,5525,5530,5535,5540,5545,5550,5555,5560,5565,5570,5575,5580,5585,5590,5595,5600,5605,5610,5615,5620,5625,5630,5635,5640,5645,5650,5655,5660,5665,5670,5675,5680,5685,5690,5695,5700,5705,5710,5715,5720,5725,5730,5735,5740,5745,5750,5755,5760,5765,5770,5775,5780,5785,5790,5795,5800,5805,5810,5815,5820,5825,5830,5835,5840,5845,5850,5855,5860,5865,5870,5875' /tmp/system.cfg && cfgmtd -f /tmp/system.cfg -w && save && ";
            }

            //Save and reboot
            Command += "save";

            return Command;

        }

    }

}