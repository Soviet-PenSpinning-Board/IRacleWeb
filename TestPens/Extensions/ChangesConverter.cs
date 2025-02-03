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
        public override ChangeBaseDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            ChangeNoneDto message = JsonSerializer.Deserialize<ChangeNoneDto>(ref reader, options)!;
            if (message.Type.GetDtoType() is not Type type)
            {
                return null;
            }

            return JsonSerializer.Deserialize(ref oldReader, type, options) as ChangeBaseDto;
        }

        public override void Write(Utf8JsonWriter writer, ChangeBaseDto value, JsonSerializerOptions options)
        {
            if (value.Type.GetDtoType() is not Type type)
            {
                type = typeof(ChangeNoneDto);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }

    public class ChangesModelConverter : JsonConverter<ChangeBaseModel>
    {
        public override ChangeBaseModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader oldReader = reader;
            ChangeNoneModel message = JsonSerializer.Deserialize<ChangeNoneModel>(ref reader, options)!;
            if (message.Type.GetModelType() is not Type type)
            {
                return null;
            }

            return JsonSerializer.Deserialize(ref oldReader, type, options) as ChangeBaseModel;
        }

        public override void Write(Utf8JsonWriter writer, ChangeBaseModel value, JsonSerializerOptions options)
        {
            if (value.Type.GetModelType() is not Type type)
            {
                type = typeof(ChangeNoneModel);
            }

            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
