using System.Collections.Generic;

namespace Nels.SemanticKernel.Extensions;

public static class ObjectExtension
{
    public static void AddOrUpdate(this Dictionary<string, object> dictionary, string key, object value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value; return;
        }
        dictionary.Add(key, value);
    }
}
