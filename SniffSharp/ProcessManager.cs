using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SniffSharp;

public class ProcessManager
{
    public string GetFilterForProcess(string processName = "Minecraft.Windows")
    {
        var process = GetProcessByName(processName);
        if (process is null)
        {
            Console.WriteLine("Minecraft Bedrock ist nicht aktiv.");
            return string.Empty;
        }

        int pid = process.Id;
        Console.WriteLine($"Minecraft Bedrock PID: {pid}");
        
        var ports = GetPortsByProcessId(pid);

        if (ports.Any())
        {
            Console.WriteLine("Gefundene Ports:");
            foreach (var port in ports)
            {
                Console.WriteLine($"Port: {port}");
            }

            // Baue einen Filter-String für SharpPcap basierend auf den ermittelten Ports
            string filter = string.Join(" or ", ports.Select(port => $"udp port {port}"));
            Console.WriteLine($"Angewendeter Filter: {filter}");

            return filter;
        }

        Console.WriteLine("Keine aktiven Ports für Minecraft Bedrock gefunden.");
        return string.Empty;
    }
    
    public List<Process> GetAllProcesses()
    {
        return Process.GetProcesses().ToList();
    }

    public Process GetCurrentProcess()
    {
        return Process.GetCurrentProcess();
    }

    public Process? GetProcessByName(string processName)
    {
        return Process.GetProcessesByName(processName).FirstOrDefault();
    }
    
    static List<int> GetPortsByProcessId(int pid)
    {
        var ports = new List<int>();

        // Run the netstat command with parameters to get PID information
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "netstat",
                Arguments = "-ano -p UDP",  // Get UDP connections with process IDs
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        var regex = new Regex(@"\s*UDP\s+\d+\.\d+\.\d+\.\d+:(\d+)\s+\*\:\*\s+(\d+)");
        
        foreach (Match match in regex.Matches(output))
        {
            int port = int.Parse(match.Groups[1].Value);
            int processId = int.Parse(match.Groups[2].Value);

            if (processId == pid)
            {
                ports.Add(port);
            }
        }

        return ports;
    }
}