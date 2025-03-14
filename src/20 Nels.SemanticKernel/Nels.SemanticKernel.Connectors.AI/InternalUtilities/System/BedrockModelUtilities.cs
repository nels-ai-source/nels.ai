using System;
using System.Collections.Generic;

namespace Nels.SemanticKernel.InternalUtilities.System;

internal class BedrockModelUtilities
{
    /// <summary>
    /// Gets the prompt execution settings extension data for the model request body build.
    /// Returns null if the extension data value is not set (default is null if TValue is a nullable type).
    /// </summary>
    /// <param name="extensionData">The execution settings extension data.</param>
    /// <param name="key">The key name of the settings parameter</param>
    /// <typeparam name="TValue">The value of the settings parameter</typeparam>
    /// <returns>The conversion to the given value of the data for execution settings</returns>
    internal static TValue? GetExtensionDataValue<TValue>(IDictionary<string, object>? extensionData, string key)
    {
        if (extensionData?.TryGetValue(key, out object? value) == true)
        {
            try
            {
                return (TValue)value;
            }
            catch (InvalidCastException)
            {
                // Handle the case where the value cannot be cast to TValue
                return default;
            }
        }

        // As long as TValue is nullable this will be properly set to null
        return default;
    }
}
