namespace ToDoList.Utils
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using ToDoList.Infrastructure.Common.Exceptions;

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd/MM/yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var stringValue = reader.GetString();
                return DateTime.ParseExact(stringValue, Format, null);
            }
            catch
            {
                throw new BusinessException("Data inválida! O formato deve ser dd/MM/yyyy");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

}
