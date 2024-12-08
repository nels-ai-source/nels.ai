using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

[Serializable]
public class ApplicationLocalizationResourceDto
{
    public Dictionary<string, string> Texts { get; set; }

    public string[] BaseResources { get; set; }
}