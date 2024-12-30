using Microsoft.SemanticKernel.Process.Models;
using Nels.SemanticKernel.Process.Steps;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.Aigc.Dtos;

public class AgentLlmStateDto : AgentMetadataBaseeDto
{
    [JsonPropertyName("llmStepState")]
    public virtual LlmStepState LlmStepState { get; set; } = new();

    [JsonPropertyName("messageStepState")]
    public virtual MessageStepState MessageStepState { get; set; } = new();
}
public class AgentMetadataBaseeDto
{
}