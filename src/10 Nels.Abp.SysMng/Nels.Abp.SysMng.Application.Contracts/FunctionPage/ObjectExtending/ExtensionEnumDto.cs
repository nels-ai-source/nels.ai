﻿using System;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionEnumDto
{
    public List<ExtensionEnumFieldDto> Fields { get; set; }

    public string LocalizationResource { get; set; }
}
