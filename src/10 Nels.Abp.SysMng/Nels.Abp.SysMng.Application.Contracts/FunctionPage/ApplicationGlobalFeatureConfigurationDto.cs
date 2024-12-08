using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

[Serializable]
public class ApplicationGlobalFeatureConfigurationDto
{
    public HashSet<string> EnabledFeatures { get; set; }

    public ApplicationGlobalFeatureConfigurationDto()
    {
        EnabledFeatures = new HashSet<string>();
    }
}
