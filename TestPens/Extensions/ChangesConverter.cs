namespace TestPens.Extensions
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using TestPens.Models.Dto;
    using TestPens.Models.Dto.Changes;
    using TestPens.Models.Real.Changes;
    using TestPens.Models.Simple;

    public class ChangesDtoConverter : JsonConverter<ChangeBaseDto>
    {
        private readonly Dictionary<ChangeType, Type> types = new Dictionary<ChangeType, Type>
        {
            { ChangeType.None, typeof(NoneChangeDto) },
            { ChangeType.ChangePosition, typeof(PositionChangeDto) },
            { ChangeType.GlobalPerson, typeof(GlobalPersonChangeDto) },
            { ChangeType.PersonProperties, typeof(PersonPropertiesChangeDto) },
        };

        public override ChangeBaseDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            NoneChangeDto message = JsonSerializer.Deserialize<NoneChangeDto>(ref reader, options)!;
            if (!types.TryGetValue(message.Type, out Type? type))
            {
                return message;
            }

            return JsonSerializer.Deserialize(ref oldReader, type, options) as ChangeBaseDto;
        }

        public override void Write(Utf8JsonWriter writer, ChangeBaseDto value, JsonSerializerOptions options)
        {
            if (!types.TryGetValue(value.Type, out Type? type))
            {
                type = typeof(NoneChangeDto);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }

    public class ChangesModelConverter : JsonConverter<ChangeBaseModel>
    {
        private readonly Dictionary<ChangeType, Type> types = new Dictionary<ChangeType, Type>
        {
            { ChangeType.None, typeof(NoneChangeModel) },
            { ChangeType.ChangePosition, typeof(PositionChangeModel) },
            { ChangeType.GlobalPerson, typeof(GlobalPersonChangeModel) },
            { ChangeType.PersonProperties, typeof(PersonPropertiesChangeModel) },
        };

        public override ChangeBaseModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            NoneChangeModel message = JsonSerializer.Deserialize<NoneChangeModel>(ref reader, options)!;
            if (!types.TryGetValue(message.Type, out Type? type))
            {
                return message;
            }

            return JsonSerializer.Deserialize(ref oldReader, type, options) as ChangeBaseModel;
        }

        public override void Write(Utf8JsonWriter writer, ChangeBaseModel value, JsonSerializerOptions options)
        {
            if (!types.TryGetValue(value.Type, out Type? type))
            {
                type = typeof(NoneChangeModel);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
