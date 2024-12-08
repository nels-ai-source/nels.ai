using System;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionPropertyApiCreateDto
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsAvailable { get; set; } = true;
}
