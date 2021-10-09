using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET
{
    public class Device
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Channel { get; set; }
        public string Power { get; set; }
        public string MaxPower { get; set; }
        public string HttpsPort { get; set; }

        public List<string> ChannelList = new List<string>();

    }
}
