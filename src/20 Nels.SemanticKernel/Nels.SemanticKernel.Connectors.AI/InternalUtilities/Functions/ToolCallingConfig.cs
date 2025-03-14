using Microsoft.SemanticKernel;
using System.Collections.Generic;

namespace Nels.SemanticKernel.InternalUtilities.Functions;

internal sealed class ToolCallingConfig(IList<ChatTool>? tools, ChatToolChoice? choice, bool autoInvoke, bool allowAnyRequestedKernelFunction, FunctionChoiceBehaviorOptions? options)
{
    public IList<ChatTool>? Tools { get; set; } = tools;

    public ChatToolChoice? Choice { get; set; } = choice;

    public bool AutoInvoke { get; set; } = autoInvoke;

    public bool AllowAnyRequestedKernelFunction { get; set; } = allowAnyRequestedKernelFunction;

    public FunctionChoiceBehaviorOptions? Options { get; set; } = options;
}