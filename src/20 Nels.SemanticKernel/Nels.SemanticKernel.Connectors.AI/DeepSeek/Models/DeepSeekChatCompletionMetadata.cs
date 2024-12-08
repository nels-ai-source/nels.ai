// Copyright (c) Microsoft. All rights reserved.

using Nels.SemanticKernel.InternalUtilities.Models;
using System.Collections.Generic;

namespace Nels.SemanticKernel.DeepSeek.Models;

/// <summary>
/// Represents the metadata of a DashScope chat completion.
/// </summary>
public sealed class DeepSeekChatCompletionMetadata : ChatCompletionMetadata
{
    internal DeepSeekChatCompletionMetadata() : base(new Dictionary<string, object?>()) { }

    private DeepSeekChatCompletionMetadata(IDictionary<string, object?> dictionary) : base(dictionary) { }
}
