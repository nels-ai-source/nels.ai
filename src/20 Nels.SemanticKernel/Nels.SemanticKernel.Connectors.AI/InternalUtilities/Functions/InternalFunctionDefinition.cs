using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;

namespace Nels.SemanticKernel.InternalUtilities.Functions;

internal class InternalFunctionDefinition
{
    /// <summary>
    /// The parameters to the function, formatting as a JSON Schema object.
    /// </summary>
    internal BinaryData Parameters;

    internal IDictionary<string, BinaryData> SerializedAdditionalRawData { get; set; }
    public InternalFunctionDefinition(string name)
    {
        Verify.NotNull(name, nameof(name));

        Name = name;
    }

    internal InternalFunctionDefinition(string description, string name, BinaryData parameters, bool? strict, IDictionary<string, BinaryData> serializedAdditionalRawData)
    {
        Description = description;
        Name = name;
        Parameters = parameters;
        Strict = strict;
        SerializedAdditionalRawData = serializedAdditionalRawData;
    }

    internal InternalFunctionDefinition()
    {
    }

    public string Description { get; set; }
    public string Name { get; set; }
    public bool? Strict { get; set; }
}
