using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class NetManager
{
    public static bool ping(string ip)
    {
        Ping ping = new Ping();
        int timeout = 5000;
        try
        {
            PingReply pingReply = ping.Send(ip, timeout);
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private static IPAddress ipAddress1;

    private static bool IsIPAddress(string addrString)
    {
        IPAddress address = (IPAddress)null;
        return IPAddress.TryParse(addrString, out address);
    }

    public static IPAddress GetIPAddress(string server)
    {
        return !IsIPAddress(server) ? GetIPAddressFromHostname(server) : IPAddress.Parse(server);
    }

    public static IPAddress GetIPAddressFromHostname(string hostname)
    {
        try
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                if (hostEntry != null)
                    ipAddress1 = hostEntry.AddressList[0];
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
            int index = 0;
            while (index < hostAddresses.Length)
            {
                IPAddress ipAddress2 = hostAddresses[index];
                if (ipAddress2.AddressFamily == AddressFamily.InterNetwork)
                {
                    string Left = ipAddress2.ToString();
                    if (Operators.CompareString(Left, "0.0.0.0", false) != 0 || Operators.CompareString(Left, "127.0.0.1", false) != 0)
                    {
                        ipAddress1 = ipAddress2;
                        break;
                    }
                }
                checked { ++index; }
            }
        }
        catch (Exception ex)
        {
        }
        return ipAddress1;
    }
    private static bool CheckVPN(string adapterId)
    {
        NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface networkInterface in allNetworkInterfaces)
        {
            if (networkInterface.Id == adapterId && networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    try
                    {
                        if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string currentVPNip = unicastAddress.Address.ToString();
                            return true;
                        }
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
            else if (networkInterface.Id == adapterId && networkInterface.OperationalStatus != OperationalStatus.Up)
            {
                return false;
            }
        }
        return false;
    }

    public static string DefaultEthernetAdapterId()
    {
        string result = "";
        NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        for (int i = 0; i < allNetworkInterfaces.Length; i++)
        {
            if (allNetworkInterfaces[i].Description.ToLower().Contains("tap") && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                result = allNetworkInterfaces[i].Id;
                break;
            }
        }
        return result;
    }

    public static bool Status(string adapterId)
    {
        return CheckVPN(adapterId);
    }

    public static string MaskedIP(string ip)
    {
        return Regex.Replace(ip, "\\d", "X").ToString();
    }
    public static string GetIp()
    {
        string hostname = Dns.GetHostName();
        IPHostEntry iphe = Dns.GetHostEntry(hostname);
        IPAddress ipaddress = null;
        foreach (IPAddress item in iphe.AddressList)
        {
            if (item.AddressFamily == AddressFamily.InterNetwork)
            {
                ipaddress = item;
            }
        }
        return ipaddress.ToString();
    }
}

