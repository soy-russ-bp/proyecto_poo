using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ARJE.Utils.Json
{
    public class TypeInfoConverter : JsonConverter
    {
        public TypeInfoConverter(Type typeFilter)
        {
            ArgumentNullException.ThrowIfNull(typeFilter);

            this.TypeFilter = typeFilter;
        }

        public TypeInfoConverter(Type typeFilter, string typePropertyName, string valuePropertyName)
        {
            ArgumentNullException.ThrowIfNull(typeFilter);
            ArgumentNullException.ThrowIfNull(typePropertyName);
            ArgumentNullException.ThrowIfNull(valuePropertyName);

            this.TypeFilter = typeFilter;
            this.TypePropertyName = typePropertyName;
            this.ValuePropertyName = valuePropertyName;
        }

        public Type TypeFilter { get; }

        public string TypePropertyName { get; } = "Type";

        public string ValuePropertyName { get; } = "Object";

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            ArgumentNullException.ThrowIfNull(value);

            JObject container = this.CreateContainer(value);
            container.WriteTo(writer, serializer.Converters.ToArray());
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var container = JObject.Load(reader);
            string typeName = container[this.TypePropertyName]?.Value<string>()
                ?? throw new JsonException();
            JToken typeValue = container[this.ValuePropertyName]
                ?? throw new JsonException();
            Type type = Type.GetType(typeName, throwOnError: true)!;
            if (!this.CanConvert(type))
            {
                throw new InvalidOperationException($"Can not convert {type}.");
            }

            return typeValue.ToObject(type);
        }

        public override bool CanConvert(Type objectType)
        {
            return this.TypeFilter.IsAssignableFrom(objectType);
        }

        private JObject CreateContainer(object value)
        {
            string? typeName = value.GetType().AssemblyQualifiedName
                ?? throw new NotSupportedException($"Assembly qualified name of type \"{value.GetType()}\" is null.");
            var typeValue = JToken.FromObject(value);
            return new JObject()
            {
                new JProperty(this.TypePropertyName, typeName),
                new JProperty(this.ValuePropertyName, typeValue),
            };
        }
    }
}
