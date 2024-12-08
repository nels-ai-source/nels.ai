using System.Collections.Generic;
using Microsoft.SemanticKernel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nels.SemanticKernel.InternalUtilities.Text;

namespace Nels.SemanticKernel.Kimi;

public sealed class KimiPromptExecutionSettings : PromptExecutionSettings
{
    public static KimiPromptExecutionSettings FromExecutionSettings(PromptExecutionSettings executionSettings)
    {
        switch (executionSettings)
        {
            case null:
                return new KimiPromptExecutionSettings();
            case KimiPromptExecutionSettings settings:
                return settings;
        }

        var json = JsonSerializer.Serialize(executionSettings);
        var kimiPromptExecutionSettings = JsonSerializer.Deserialize<KimiPromptExecutionSettings>(json, JsonOptionsCache.ReadPermissive);

        return kimiPromptExecutionSettings!;
    }





    [JsonPropertyName("temperature")]
    public float Temperature
    {
        get => _temperature;

        set
        {
            ThrowIfFrozen();
            _temperature = value;
        }
    }


    /// <summary>
    /// The maximum number of tokens to generate in the completion.
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens
    {
        get => _maxTokens;

        set
        {
            ThrowIfFrozen();
            _maxTokens = value;
        }
    }


    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens.
    /// </summary>
    [JsonPropertyName("stop")]
    public List<string> Stop
    {
        get => _stop;

        set
        {
            ThrowIfFrozen();
            _stop = value;
        }
    }

    [JsonPropertyName("reultsPerPrompt")]
    public int ResultsPerPrompt
    {
        get => _resultsPerPrompt;

        set
        {
            ThrowIfFrozen();
            _resultsPerPrompt = value;
        }
    }




    public override PromptExecutionSettings Clone()
    {
        return new KimiPromptExecutionSettings()
        {
            ModelId = ModelId,
            Temperature = Temperature,
            MaxTokens = MaxTokens,
            ResultsPerPrompt = ResultsPerPrompt,
            Stop = Stop != null ? new List<string>(Stop) : null,

        };
    }

    private List<string> _stop;
    private int _resultsPerPrompt = 1;
    private float _temperature = 1;
    private int? _maxTokens;

}
