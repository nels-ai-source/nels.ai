// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Nels.SemanticKernel.InternalUtilities.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DeepSeek;

/// <summary>
/// DashScope Execution Settings.
/// </summary>
public sealed class DeepSeekPromptExecutionSettings : PromptExecutionSettings
{
    /// <summary>
    /// Gets the specialization for the Qwen execution settings.
    /// </summary>
    /// <param name="executionSettings">Generic prompt execution settings.</param>
    /// <returns>Specialized Qwen execution settings.</returns>
    public static DeepSeekPromptExecutionSettings FromExecutionSettings(PromptExecutionSettings executionSettings)
    {
        switch (executionSettings)
        {
            case null:
                return new DeepSeekPromptExecutionSettings();
            case DeepSeekPromptExecutionSettings settings:
                return settings;
        }

        var json = JsonSerializer.Serialize(executionSettings);
        var dashScopePromptExecutionSettings = JsonSerializer.Deserialize<DeepSeekPromptExecutionSettings>(json, JsonOptionsCache.ReadPermissive);

        return dashScopePromptExecutionSettings!;
    }

    /// <summary>
    /// (Default: 1.0). Float (0.0-100.0). The temperature of the sampling operation. 1 means regular sampling,
    /// 0 means always take the highest score, 100.0 is getting closer to uniform probability.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double Temperature
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
    /// TopP controls the diversity of the completion.
    /// The higher the TopP, the more diverse the completion.
    /// Default is 1.0.
    /// </summary>
    [JsonPropertyName("top_p")]
    public double TopP
    {
        get => this._topP;

        set
        {
            this.ThrowIfFrozen();
            this._topP = value;
        }
    }
    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens
    /// based on whether they appear in the text so far, increasing the
    /// model's likelihood to talk about new topics.
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public double PresencePenalty
    {
        get => this._presencePenalty;

        set
        {
            this.ThrowIfFrozen();
            this._presencePenalty = value;
        }
    }

    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens
    /// based on their existing frequency in the text so far, decreasing
    /// the model's likelihood to repeat the same line verbatim.
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public double FrequencyPenalty
    {
        get => this._frequencyPenalty;

        set
        {
            this.ThrowIfFrozen();
            this._frequencyPenalty = value;
        }
    }
    /// <summary>
    /// Whether to return log probabilities of the output tokens or not.
    /// If true, returns the log probabilities of each output token returned in the `content` of `message`.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public bool? Logprobs
    {
        get => this._logprobs;

        set
        {
            this.ThrowIfFrozen();
            this._logprobs = value;
        }
    }

    /// <summary>
    /// An integer specifying the number of most likely tokens to return at each token position, each with an associated log probability.
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs
    {
        get => this._topLogprobs;

        set
        {
            this.ThrowIfFrozen();
            this._topLogprobs = value;
        }
    }

    /// <inheritdoc />
    public override PromptExecutionSettings Clone()
    {
        return new DeepSeekPromptExecutionSettings()
        {
            ModelId = this.ModelId,
            ExtensionData = this.ExtensionData is not null ? new Dictionary<string, object>(this.ExtensionData) : null,
            Temperature = this.Temperature,
            TopP = this.TopP,
            PresencePenalty = this.PresencePenalty,
            FrequencyPenalty = this.FrequencyPenalty,
            MaxTokens = this.MaxTokens,
            Logprobs = this.Logprobs,
            TopLogprobs = this.TopLogprobs,
        };
    }

    private double _temperature = 1;
    private double _topP = 1;
    private double _presencePenalty;
    private double _frequencyPenalty;
    private int? _maxTokens;
    private IList<string>? _stopSequences;
    private int _resultsPerPrompt = 1;
    private long? _seed;
    private object? _responseFormat;
    private IDictionary<int, int>? _tokenSelectionBiases;
    private ToolCallBehavior? _toolCallBehavior;
    private string? _user;
    private string? _chatSystemPrompt;
    private bool? _logprobs;
    private int? _topLogprobs;
}
