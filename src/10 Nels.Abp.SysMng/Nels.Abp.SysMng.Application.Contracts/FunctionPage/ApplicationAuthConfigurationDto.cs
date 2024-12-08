using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

[Serializable]
public class ApplicationAuthConfigurationDto
{
    public Dictionary<string, bool> GrantedPolicies { get; set; }

    public ApplicationAuthConfigurationDto()
    {
        GrantedPolicies = new Dictionary<string, bool>();
    }
}
