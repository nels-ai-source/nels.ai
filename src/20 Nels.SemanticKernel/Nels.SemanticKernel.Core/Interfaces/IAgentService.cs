using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nels.SemanticKernel.Core.Enums;

namespace Nels.SemanticKernel.Interfaces;

public interface IAgentService
{
    Task<IAgent> GetAgentAsync(Guid id);
}

public interface IAgent
{
    Guid Id { get; set; }
    Guid SpaceId { get; set; }
    string Name { get; set; }
    string Icon { get; set; }
    string Description { get; set; }
    AgentType Type { get; set; }
    string Instructions { get; set; }
    string Prologue { get; set; }
    List<IAgentPresetQuestions> Questions { get; }
    List<IAgentKnowledge> Knowledges { get;  }
    List<IAgentTool> Tools { get; }
}

public interface IAgentPresetQuestions
{
    Guid Id { get; set; }
    Guid AgentId { get; set; }
    string Content { get; set; }
    int Index { get; set; }
}

public interface IAgentTool
{
    Guid Id { get; set; }
    Guid AgentId { get; set; }
    Guid ToolId { get; set; }
}

public interface IAgentKnowledge
{
    Guid Id { get; set; }
    Guid AgentId { get; set; }
    Guid KnowledgeId { get; set; }
}