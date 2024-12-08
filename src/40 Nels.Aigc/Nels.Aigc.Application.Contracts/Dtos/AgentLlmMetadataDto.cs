using Microsoft.SemanticKernel.Process.Models;
using Nels.SemanticKernel.Process.Steps;
using System.Collections.Generic;

namespace Nels.Aigc.Dtos;

public class AgentLlmMetadataDto : AgentMetadataBaseeDto
{
    public virtual LlmStepState LlmStepState { get; set; } = new();
    public virtual MessageStepState MessageStepState { get; set; } = new();
}
public class AgentMetadataBaseeDto
{
}