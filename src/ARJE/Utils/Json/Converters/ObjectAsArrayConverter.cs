using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ARJE.Utils.Json.Converters
{
    public class ObjectAsArrayConverter<TObject, TElement> : JsonConverter<TObject>
        where TObject : IObjectAsArray<TElement>
    {
        public override void WriteJson(JsonWriter writer, TObject? value, JsonSerializer serializer)
        {
            JArray array = JArray.FromObject(value!.ObjectAsArray);
            array.WriteTo(writer);
        }

        public override TObject? ReadJson(JsonReader reader, Type objectType, TObject? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var arrayObject = array.ToObject<TElement[]>()!;
            return (TObject)TObject.Create(arrayObject);
        }
    }
}
