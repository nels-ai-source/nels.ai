using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

[Serializable]
public class ApplicationSettingConfigurationDto
{
    public Dictionary<string, string> Values { get; set; }
}
