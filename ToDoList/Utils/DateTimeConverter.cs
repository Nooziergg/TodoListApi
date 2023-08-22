namespace ToDoList.Utils
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd/MM/yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return DateTime.ParseExact(stringValue, Format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

}
