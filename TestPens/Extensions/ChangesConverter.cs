namespace TestPens.Extensions
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using TestPens.Models.Abstractions;
    using TestPens.Models.Changes;

    public class ChangesConverter : JsonConverter<BaseChange>
    {
        private readonly Dictionary<ChangeType, Type> types = new Dictionary<ChangeType, Type>
        {
            { ChangeType.None, typeof(NoneChange) },
            { ChangeType.ChangePosition, typeof(PositionChange) },
            { ChangeType.NewPerson, typeof(NewPersonChange) },
            { ChangeType.PersonProperties, typeof(PersonPropertiesChange) },
            { ChangeType.DeletePerson, typeof(DeletePersonChange) },
        };

        public override BaseChange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            NoneChange message = JsonSerializer.Deserialize<NoneChange>(ref reader, options)!;
            if (!types.TryGetValue(message.Type, out Type? type))
            {
                return message;
            }

            return JsonSerializer.Deserialize(ref oldReader, type, options) as BaseChange;
        }

        public override void Write(Utf8JsonWriter writer, BaseChange value, JsonSerializerOptions options)
        {
            if (!types.TryGetValue(value.Type, out Type? type))
            {
                type = typeof(NoneChange);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
