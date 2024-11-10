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
    public static List<string> ips = [];
    
    public static void Main()
    {
        SniffManager sniffManager = new SniffManager();
        sniffManager.Test();

        Console.ReadLine();
    }
}