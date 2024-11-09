namespace DiscordInteractor;

class Program
{
    static void Main(string[] args)
    {
                public List<Message> GetChannelMessages(long channel_id, int message_count = 100)
                {
                    var request = new RestRequest($"https://discord.com/api/v9/channels/{channel_id}/messages?limit={message_count}", Method.Get);
        
                    var response = Global.client.Execute(Global.AddHeader(request));
        
                    JArray message_array = JArray.Parse(response.Content.Replace("\"", "'"));
        
                    List<Message> message_list = new List<Message>();
        
                    foreach (JObject json in message_array)
                    {
                        message_list.Add(Global.ConvertJsonToMessage(json));
                    }
                    return message_list;
                }
    }
}
