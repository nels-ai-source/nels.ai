using Nels.SemanticKernel.Extensions;
using System.Collections.Generic;

namespace Nels.SemanticKernel.Process.Extensions;

public static class ObjectExtension
{
    public static void AddDefaultOutput(this Dictionary<string, object> dictionary, string key, object value)
    {
        var output = new Dictionary<string, object> { { StepConst.DefaultOutput, value } };
        dictionary.AddOrUpdate(key, output);
    }
}
