using System.IO;
using Newtonsoft.Json;

namespace ARJE.Utils.Json
{
    public static class JsonWrite
    {
        public static void ToFile<T>(string filePath, T objectToWrite, bool indented = false)
        {
            Formatting formatting = indented ? Formatting.Indented : Formatting.None;
            string objectJson = JsonConvert.SerializeObject(objectToWrite, formatting);
            File.WriteAllText(filePath, objectJson);
        }
    }
}
