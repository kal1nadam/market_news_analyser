using System.Text.Json.Serialization;
using NewsAnalyzer.Application.Common.Convertors;

namespace NewsAnalyzer.Application.DTO.External;

public sealed class ImportNewsDto
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("publishedDate")]
    [JsonConverter(typeof(NewsPublishedDateTimeConvertor))]
    public DateTime PublishedDate { get; set; }

    [JsonPropertyName("publisher")]
    public string Publisher { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("site")]
    public string Site { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}