using System.IO;
using Newtonsoft.Json;

namespace LastBell.Managers;

public class JsonManager
{
    public T? ReadJson<T>(string path)
    {
        var jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path + ".json"));
        return JsonConvert.DeserializeObject<T>(jsonContent);
    }
}