using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;

namespace Nels.SemanticKernel.InternalUtilities.Functions;

internal partial class InternalChatCompletionNamedToolChoiceFunction
{
    internal IDictionary<string, BinaryData> SerializedAdditionalRawData { get; set; }
    public InternalChatCompletionNamedToolChoiceFunction(string name)
    {
        Verify.NotNull(name, nameof(name));

        Name = name;
    }

    internal InternalChatCompletionNamedToolChoiceFunction(string name, IDictionary<string, BinaryData> serializedAdditionalRawData)
    {
        Name = name;
        SerializedAdditionalRawData = serializedAdditionalRawData;
    }

    internal InternalChatCompletionNamedToolChoiceFunction()
    {
    }

    public string Name { get; }
}
