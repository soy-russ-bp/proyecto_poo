using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ARJE.Utils.Json
{
    public static class JsonRead
    {
        public static T FromFile<T>(string filePath)
        {
            string objectJson = File.ReadAllText(filePath);
            T? @object = JsonConvert.DeserializeObject<T>(objectJson);
            return @object == null
                ? throw new SerializationException($"Error on deserialization of type: {nameof(T)}. File path: {filePath}")
                : @object;
        }
    }
}
