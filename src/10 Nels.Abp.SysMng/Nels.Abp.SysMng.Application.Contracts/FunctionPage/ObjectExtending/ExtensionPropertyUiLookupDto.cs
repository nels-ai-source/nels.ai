﻿using System;

namespace Nels.Abp.SysMng.FunctionPage.ObjectExtending;

[Serializable]
public class ExtensionPropertyUiLookupDto
{
    public string Url { get; set; }
    public string ResultListPropertyName { get; set; }
    public string DisplayPropertyName { get; set; }
    public string ValuePropertyName { get; set; }
    public string FilterParamName { get; set; }
}
