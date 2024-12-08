using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class LocalizableStringDto([NotNull] string name, string resource = null)
{
    [NotNull]
    public string Name { get; private set; } = Check.NotNullOrEmpty(name, nameof(name));

    [CanBeNull]
    public string Resource { get; set; } = resource;
}
