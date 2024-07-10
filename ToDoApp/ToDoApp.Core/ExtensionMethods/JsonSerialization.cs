using System.Text.Json;

namespace ToDoApp.Core.ExtensionMethods;

public static class JsonSerialization
{

    public  static string Serialize(this object obj )
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T? Deserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

}
