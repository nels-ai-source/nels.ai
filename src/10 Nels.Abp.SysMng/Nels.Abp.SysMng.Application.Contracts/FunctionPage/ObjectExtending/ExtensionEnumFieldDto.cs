using System;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionEnumFieldDto
{
    public string Name { get; set; }

    public object Value { get; set; }
}
