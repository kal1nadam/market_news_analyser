using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewsAnalyzer.Application.Common.Convertors;

/// <summary>
/// Custom JSON converter for DateTime to handle news published date returned from FMP external api.
/// </summary>
public class NewsPublishedDateTimeConvertor : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString() ?? string.Empty, Format, null);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}