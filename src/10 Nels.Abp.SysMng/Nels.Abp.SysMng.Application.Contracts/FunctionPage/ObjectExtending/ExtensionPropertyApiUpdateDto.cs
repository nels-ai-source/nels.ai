using System;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionPropertyApiUpdateDto
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsAvailable { get; set; } = true;
}
