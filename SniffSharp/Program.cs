using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using PacketDotNet;
using PacketDotNet.DhcpV4;
using SharpPcap;
using SharpPcap.LibPcap;

namespace SniffSharp;

class Program
{
    public static void Main()
    {
        SniffManager sniffManager = new SniffManager();
        sniffManager.Test();

        Console.ReadLine();

        foreach (var ipData in sniffManager.GetIpData())
        {
            Console.WriteLine($"{ipData.Isp}");
        }
        
        foreach (var ipData in sniffManager.GetIpData())
        {
            Console.WriteLine($"{ipData.IPAddress} - {ipData.Isp}");
        }
    }
}