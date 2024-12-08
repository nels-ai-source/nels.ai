using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionPropertyAttributeDto
{
    public string TypeSimple { get; set; }

    public Dictionary<string, object> Config { get; set; }
}
