// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nels.SemanticKernel.InternalUtilities.Models;

/// <summary>
/// Represents the metadata of a DashScope chat completion.
/// </summary>
public class ChatCompletionMetadata : ReadOnlyDictionary<string, object>
{
    public ChatCompletionMetadata() : base(new Dictionary<string, object>()) { }

    public ChatCompletionMetadata(IDictionary<string, object> dictionary) : base(dictionary) { }


    /// <summary>
    /// Creation time of the response.
    /// </summary>
    public long? Created
    {
        get => GetValueFromDictionary(nameof(Created)) as long? ?? 0;
        set => SetValueInDictionary(value, nameof(Created));
    }

    /// <summary>
    /// Reason why the processing was finished.
    /// </summary>
    public string FinishReason
    {
        get => GetValueFromDictionary(nameof(FinishReason)) as string;
        set => SetValueInDictionary(value, nameof(FinishReason));
    }


    /// <summary>
    /// The total count of tokens used.
    /// </summary>
    /// <remarks>
    /// Usage is not available for streaming chunks.
    /// </remarks>
    public int? UsageTotalTokens
    {
        get => GetValueFromDictionary(nameof(UsageTotalTokens)) as int?;
        set => SetValueInDictionary(value, nameof(UsageTotalTokens));
    }

    /// <summary>
    /// The count of tokens in the prompt.
    /// </summary>
    /// <remarks>
    /// Usage is not available for streaming chunks.
    /// </remarks>
    public int? UsagePromptTokens
    {
        get => GetValueFromDictionary(nameof(UsagePromptTokens)) as int?;
        set => SetValueInDictionary(value, nameof(UsagePromptTokens));
    }

    /// <summary>
    /// The count of token in the current completion.
    /// </summary>
    /// <remarks>
    /// Usage is not available for streaming chunks.
    /// </remarks>
    public int? UsageCompletionTokens
    {
        get => GetValueFromDictionary(nameof(UsageCompletionTokens)) as int?;
        set => SetValueInDictionary(value, nameof(UsageCompletionTokens));
    }


    /// <summary>
    /// Converts a dictionary to a <see cref="ChatCompletionMetadata"/> object.
    /// </summary>
    public static ChatCompletionMetadata FromDictionary(IReadOnlyDictionary<string, object> dictionary) => dictionary switch
    {
        null => throw new ArgumentNullException(nameof(dictionary)),
        ChatCompletionMetadata metadata => metadata,
        IDictionary<string, object> metadata => new ChatCompletionMetadata(metadata),
        _ => new ChatCompletionMetadata(dictionary.ToDictionary(pair => pair.Key, pair => pair.Value))
    };

    public void SetValueInDictionary(object value, string propertyName)
        => Dictionary[propertyName] = value;

    public object GetValueFromDictionary(string propertyName)
        => Dictionary.TryGetValue(propertyName, out var value) ? value : null;
}
