namespace SKYNET.Models
{
    public class DeviceClient
    {
        public string mac { get; set; }
        public string name { get; set; }
        public string lastip { get; set; }
        public string associd { get; set; }
        public string aprepeater { get; set; }
        public decimal tx { get; set; }
        public decimal rx { get; set; }
        public int signal { get; set; }
        public string rssi { get; set; }
        public int[] chainrssi { get; set; }
        public string rx_chainmask { get; set; }
        public string ccq { get; set; }
        public string idle { get; set; }
        public string tx_latency { get; set; }
        public string uptime { get; set; }
        public string ack { get; set; }
        public string distance { get; set; }
        public string txpower { get; set; }
        public string noisefloor { get; set; }
        public long[] tx_ratedata { get; set; }
        public airMAX airmax { get; set; }
        public Stats stats { get; set; }
        public string[] rates { get; set; }
        public int[] signals { get; set; }
        public DeviceClient remote { get; set; }

    }

    public class airMAX
    {
        public int priority { get; set; }
        public int quality { get; set; }
        public int beam { get; set; }
        public int signal { get; set; }
        public int capacity { get; set; }
    }
    public class Stats
    {
        public long rx_data { get; set; }
        public long rx_bytes { get; set; }
        public long rx_pps { get; set; }
        public long tx_data { get; set; }
        public long tx_bytes { get; set; }
        public long tx_pps { get; set; }
    }
}