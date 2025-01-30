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
        private static readonly Dictionary<ChangeType, Type> types = new Dictionary<ChangeType, Type>
        {
            { ChangeType.None, typeof(ChangeNoneDto) },
            { ChangeType.ChangePosition, typeof(ChangePositionDto) },
            { ChangeType.GlobalPerson, typeof(ChangeGlobalPersonDto) },
            { ChangeType.PersonProperties, typeof(ChangePersonPropertiesDto) },
        };

        public override ChangeBaseDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            ChangeNoneDto message = JsonSerializer.Deserialize<ChangeNoneDto>(ref reader, options)!;
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
                type = typeof(ChangeNoneDto);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }

    public class ChangesModelConverter : JsonConverter<ChangeBaseModel>
    {
        private static Dictionary<ChangeType, Type> types = new Dictionary<ChangeType, Type>
        {
            { ChangeType.None, typeof(ChangeNoneModel) },
            { ChangeType.ChangePosition, typeof(ChangePositionModel) },
            { ChangeType.GlobalPerson, typeof(ChangeGlobalPersonModel) },
            { ChangeType.PersonProperties, typeof(ChangePersonPropertiesModel) },
        };

        public override ChangeBaseModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            ChangeNoneModel message = JsonSerializer.Deserialize<ChangeNoneModel>(ref reader, options)!;
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
                type = typeof(ChangeNoneModel);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
