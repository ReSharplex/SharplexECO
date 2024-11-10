using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using PacketDotNet;
using SharpPcap;

namespace SniffSharp;

public class SniffManager
{
    private readonly IpManager _ipManager = new();
    private readonly ProcessManager _processManager = new();

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

    private Process GetCurrentAppProcess()
    {
        IntPtr activeAppHandle = GetForegroundWindow();

        IntPtr activeAppProcessId;
        GetWindowThreadProcessId(activeAppHandle, out activeAppProcessId);
        
        Process currentAppProcess = Process.GetProcessById((int)activeAppProcessId);
        return currentAppProcess;
    }
    
    public void Test()
    {
        ListenToDevice(4, _processManager.GetFilterForProcess());
    }
    
    public void ListenToDevice(int deviceIndex = 4, string? filter = null)
    {
        var devices = CaptureDeviceList.Instance;
        var device = devices[deviceIndex];
        int readTimeoutMilliseconds = 1000;
        device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

        if (!string.IsNullOrEmpty(filter))
        {
            device.Filter = filter;
        }
        
        device.OnPacketArrival += Device_OnPacketArrival;
        device.StartCapture();
    }

    private void Device_OnPacketArrival(object sender, PacketCapture e)
    {
        var rawPacket = e.GetPacket();
        DateTime time = e.Header.Timeval.Date;
        var len = e.Data.Length;
        Task.Run(async () => await ProcessPacketAsync(rawPacket, time, len));
    }

    private async Task ProcessPacketAsync(RawCapture rawPacket, DateTime time, int len)
    {
        Packet? packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        await ExtractPacket(packet, time, len);
    }

    private async Task ExtractPacket(Packet packet, DateTime time, int len)
    {
        if (!await ExtractTcpPacket(packet, time, len)) return;
        if (!await ExtractUdpPacket(packet, time, len)) return;
    }
    
    private async Task<bool> ExtractTcpPacket(Packet packet, DateTime time, int len)
    {
        var tcpPacket = packet.Extract<TcpPacket>();
        if (tcpPacket == null) return false;
        
        IPPacket? ipPacket = GetIpPacket(tcpPacket);
        if (ipPacket is null) return false;
        
        var ipData = await GetInformationAboutIp(ipPacket.DestinationAddress);

        ShowInformation(ipData, ipPacket, tcpPacket, time, len);
        
        return true;
    }

    private async Task<bool> ExtractUdpPacket(Packet packet, DateTime time, int len)
    {
        UdpPacket? udpPacket = packet.Extract<UdpPacket>();
        if (udpPacket is null) return false;
        
        IPPacket? ipPacket = GetIpPacket(udpPacket);
        if (ipPacket is null) return false;
        
        var ipData = await GetInformationAboutIp(ipPacket.DestinationAddress);
        
        ShowInformation(ipData, ipPacket, udpPacket, time, len);
        
        return true;
    }

    private async Task<IpData> GetInformationAboutIp(IPAddress ipAddress)
    {
        return await _ipManager.Parse(ipAddress);
    }

    private void ShowInformation(IpData ipData, IPPacket ipPacket, TransportPacket packet, DateTime time, int len)
    {
        if (string.IsNullOrEmpty(ipData.Isp)) return;
        Console.WriteLine($"{time.Minute}:{time.Second} - {len} [{ipPacket.SourceAddress}:{packet.SourcePort} -> {ipPacket.DestinationAddress}:{packet.DestinationPort}] - {ipData.Isp}");
    }

    private IPPacket? GetIpPacket<TSource>(TSource packet) where TSource : Packet
    {
        return packet.ParentPacket as IPPacket;
    }
}