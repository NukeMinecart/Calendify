using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Calendify.client;

public static class ClientSecretRetriver
{
    public static string? GetJsonValue(string key)
    {
        var json = new StreamReader(new FileStream("client/client_secrets.json", FileMode.Open)).ReadToEnd();
        var data = (JObject)JsonConvert.DeserializeObject(json)!;
        var value = data[key]!.Value<string>();
        return value;
    }
}