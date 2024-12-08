// Copyright (c) Microsoft. All rights reserved.

using Nels.SemanticKernel.InternalUtilities.Models;
using System.Collections.Generic;

namespace Nels.SemanticKernel.Kimi.Models;

/// <summary>
/// Represents the metadata of a Hugging Face chat completion.
/// </summary>
public sealed class KimiChatCompletionMetadata : ChatCompletionMetadata
{
    internal KimiChatCompletionMetadata() : base(new Dictionary<string, object>()) { }

    private KimiChatCompletionMetadata(IDictionary<string, object> dictionary) : base(dictionary) { }
}
