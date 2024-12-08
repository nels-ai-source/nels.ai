﻿using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Nels;

[Dependency(ReplaceServices = true)]
public class BrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Nels";
}
