using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Nels.SemanticKernel.Extensions;

public static class EnumExtensions
{
    public static List<EnumDto<TEnum>> ToEnumDtoList<TEnum>() where TEnum : Enum
    {
        List<EnumDto<TEnum>> enumDtos = [];
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            EnumDto<TEnum> dto = new()
            {
                Id = Guid.NewGuid(),
                Value = value,
                Label = GetDescription(value)
            };
            enumDtos.Add(dto);
        }
        return enumDtos;
    }
    public static Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : Enum
    {
        Dictionary<TEnum, string> keyValuePairs = [];
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            keyValuePairs.Add(value, GetDescription(value));
        }
        return keyValuePairs;
    }

    private static string GetDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                {
                    return attr.Description;
                }
            }
        }
        return value.ToString();
    }
}

public class EnumDto<TEnum> where TEnum : Enum
{
    public Guid Id { get; set; }
    public TEnum Value { get; set; }
    public string Label { get; set; }
}
