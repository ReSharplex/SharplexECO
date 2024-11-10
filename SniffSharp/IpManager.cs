using System.Net;
using Newtonsoft.Json.Linq;

namespace SniffSharp;

public class IpManager
{
    private List<IpData> _ips = [];
    
    public async Task<IpData> Parse(IPAddress ip)
    {
        var tempIp = _ips.FirstOrDefault(e => ip.Equals(e.IPAddress));
        if (tempIp is not null) return tempIp;
        
        using var client = new HttpClient();
        var response = await client.GetAsync($"http://ip-api.com/json/{ip}?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting");
        string dataJson = await response.Content.ReadAsStringAsync();

        dynamic data = JObject.Parse(dataJson);

        var ipData = new IpData()
        {
            ParseSuccess = true,
            IPAddress = ip,
            Status = data.status,
            City = data.city,
            Country = data.country,
            CountryCode = data.countryCode,
            Postal = data.zip,
            Timezone = data.timezone,
            Latitude = data.lat,
            Longitude = data.lon,
            Isp = data.isp,
            Asn = data.@as
        };
        
        _ips.Add(ipData);
        return ipData;
    }
}

public class IpData
{
    public string Timezone { get; set; }
    public string Latitude { get; set; }
    public string Postal { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string City { get; set; }
    public string Longitude { get; set; }
    public string Isp { get; set; }
    public string Asn { get; set; }
    public bool ParseSuccess { get; set; }
    public IPAddress IPAddress { get; set; }
    public string Status { get; set; }
}