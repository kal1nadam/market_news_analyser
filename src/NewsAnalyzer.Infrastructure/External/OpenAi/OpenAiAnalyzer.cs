using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.DTO.External;
using OpenAI.Responses;

namespace NewsAnalyzer.Infrastructure.External.OpenAi;

public class OpenAiAnalyzer : IAiAnalyzer
{
    private readonly OpenAiApiOptions _options;
    private readonly ILogger _logger;
    
    private const string Gpt5MiniModel = "gpt-5-mini";
    
    public OpenAiAnalyzer(IOptions<OpenAiApiOptions> options, ILogger<OpenAiAnalyzer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }
    
    public async Task<NewsAnalysisDto?> AnalyzeNewsAsync(EnrichNewsPromptDto param, CancellationToken ct = default)
    {
        var developerMessage = await File.ReadAllTextAsync("Common/OpenAi/DeveloperMessages/NewsAnalysis.txt", ct);

        var userMessage = JsonSerializer.Serialize(param);

        var outputJsonSchema = await File.ReadAllTextAsync("Common/OpenAi/Schemas/NewsAnalysis.txt", ct);
        
        var creationOptions = new ResponseCreationOptions()
        {
            Tools = { ResponseTool.CreateWebSearchTool() },
            TextOptions = new ResponseTextOptions()
            {
                TextFormat = ResponseTextFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "NewsAiEnrichment",
                    jsonSchema: BinaryData.FromString(outputJsonSchema),
                    jsonSchemaIsStrict: true)
            }
        };
        
        var responseClient = new OpenAIResponseClient(Gpt5MiniModel, _options.ApiKey);
        
        var response = await responseClient.CreateResponseAsync(new ResponseItem[]
        {
            ResponseItem.CreateDeveloperMessageItem(developerMessage),
            ResponseItem.CreateUserMessageItem(userMessage)
        }, creationOptions, ct);
        
        // Deserialize the response
        var output = response.Value.GetOutputText();
        
        if (string.IsNullOrEmpty(output))
        {
            throw new InvalidOperationException("The AI did not return any output.");
        }

        try
        {
            var analysis = JsonSerializer.Deserialize<NewsAnalysisDto>(output);
            return analysis;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to analyze a news. Exception while deserializing output: {Output}", output);
            return null;
        }
    }
}