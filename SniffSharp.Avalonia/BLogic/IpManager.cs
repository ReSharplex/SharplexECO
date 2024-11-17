using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SniffSharp.Avalonia.Exceptions;
using SniffSharp.Avalonia.Utility;

namespace SniffSharp.Avalonia.BLogic;

public class IpManager
{
    private List<IpData> Ips = [];
    
    public ValidationResult Scan(string ip, out IpData? ipData)
    {
        var validation = TryParseIp(ip, out var address);
        if (validation.IsValid) return GetInformation(address!, out ipData);
        
        ipData = null;
        return validation;
    }

    private ValidationResult TryParseIp(string ipAsString, out IPAddress? address)
    {
        return IPAddress.TryParse(ipAsString, out address) ? new ValidationResult() : ValidationResult.Error("Invalid IP address");
    }

    private ValidationResult GetInformation(IPAddress ip, out IpData? ipData)
    {
        var tempIp = Ips.FirstOrDefault(e => ip.Equals(e.IPAddress));
        if (tempIp is not null)
        {
            ipData = tempIp;
            return ValidationResult.Success();
        };
        
        using var client = new HttpClient();
        var response = client
            .GetAsync(
                $"http://ip-api.com/json/{ip}?fields=status,continent,country,city,isp,org,as")
            .Result;
        string dataJson = response.Content.ReadAsStringAsync().Result;

        if (string.IsNullOrEmpty(dataJson))
        {
            ipData = null;
            return ValidationResult.Error("Invalid IP address");
        }
        
        dynamic data = JObject.Parse(dataJson);

        ipData = new()
        {
            ParseSuccess = true,
            IPAddress = ip,
            Status = data.status,
            City = data.city,
            Country = data.country,
            Continent = data.continent,
            Isp = data.isp,
            Org = data.org,
            As = data.@as
        };
        
        Ips.Add(ipData);
        return ValidationResult.Success();
    }
}

public class IpData
{
    public string Country { get; set; }
    public string Continent { get; set; }
    public string City { get; set; }
    public string Isp { get; set; }
    public string Org { get; set; }
    public string As { get; set; }
    public bool ParseSuccess { get; set; }
    public IPAddress IPAddress { get; set; }
    public string Status { get; set; }
}