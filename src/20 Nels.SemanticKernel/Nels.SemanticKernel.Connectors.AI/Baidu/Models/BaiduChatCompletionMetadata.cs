using Nels.SemanticKernel.InternalUtilities.Models;
using System.Collections.Generic;

namespace Nels.SemanticKernel.Baidu.Models;

public sealed class BaiduChatCompletionMetadata : ChatCompletionMetadata
{
    internal BaiduChatCompletionMetadata() : base(new Dictionary<string, object>()) { }

    private BaiduChatCompletionMetadata(IDictionary<string, object> dictionary) : base(dictionary) { }

    /// <summary>
    /// Object identifier.
    /// </summary>
#pragma warning disable CA1720 // Identifier contains type name
    public string Object
    {
        get => GetValueFromDictionary(nameof(Object)) as string;
        set => SetValueInDictionary(value, nameof(Object));
    }
#pragma warning restore CA1720 // Identifier contains type name


    /// <summary>
    /// Model used to generate the response.
    /// </summary>
    public string Model
    {
        get => GetValueFromDictionary(nameof(Model)) as string;
        set => SetValueInDictionary(value, nameof(Model));
    }
    /// <summary>
    /// System fingerprint.
    /// </summary>
    public string SystemFingerPrint
    {
        get => GetValueFromDictionary(nameof(SystemFingerPrint)) as string;
        set => SetValueInDictionary(value, nameof(SystemFingerPrint));
    }

    /// <summary>
    /// Id of the response.
    /// </summary>
    public string Id
    {
        get => GetValueFromDictionary(nameof(Id)) as string;
        set => SetValueInDictionary(value, nameof(Id));
    }

    /// <summary>
    /// The log probabilities of the completion.
    /// </summary>
    public object LogProbs
    {
        get => GetValueFromDictionary(nameof(LogProbs));
        set => SetValueInDictionary(value, nameof(LogProbs));
    }
}
