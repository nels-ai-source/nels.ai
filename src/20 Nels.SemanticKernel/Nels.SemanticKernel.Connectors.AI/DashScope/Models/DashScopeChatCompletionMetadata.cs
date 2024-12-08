// Copyright (c) Microsoft. All rights reserved.

using Nels.SemanticKernel.InternalUtilities.Models;
using System.Collections.Generic;

namespace Nels.SemanticKernel.DashScope.Models;

/// <summary>
/// Represents the metadata of a DashScope chat completion.
/// </summary>
public sealed class DashScopeChatCompletionMetadata : ChatCompletionMetadata
{
    internal DashScopeChatCompletionMetadata() : base(new Dictionary<string, object?>()) { }

    private DashScopeChatCompletionMetadata(IDictionary<string, object?> dictionary) : base(dictionary) { }
}
