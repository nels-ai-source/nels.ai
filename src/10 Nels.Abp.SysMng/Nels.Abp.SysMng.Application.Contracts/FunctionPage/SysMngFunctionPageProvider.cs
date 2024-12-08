using Nels.Abp.SysMng.Localization;
using System;
using System.Collections.Generic;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace Nels.Abp.SysMng.FunctionPage;

public class SysMngFunctionPageProvider(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer) : FunctionPageProvider
{
    public IGuidGenerator GuidGenerator = guidGenerator;
    public ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public override void Define(IFunctionPageDefinitionContext context)
    {
        //var (app, businessUnits, pages) = InitSysMngApp();
        //context.AddGroup(app, businessUnits, pages);
    }

    private (App app, List<BusinessUnit> businessUnits, List<Page> pages) InitSysMngApp()
    {
        #region 系统管理
        var app = new App(Guid.Parse("3a135b88-adf1-addb-349d-9b32f3e82937"))
        {
            Name = "SysMng",
            DisplayName = "系统管理",
            Icon = "el-icon-setting",
            Sort = "90"
        };

        var businessUnits = new List<BusinessUnit>();

        var appPageUnit = new BusinessUnit(Guid.Parse("3a135b88-adf2-ba84-079a-1fc80510c193"))
        {
            ApplicationId = app.Id,
            Name = "appPage",
            DisplayName = "应用管理",
            Icon = "el-icon-grid",
            Sort = "90",
        };
        businessUnits.Add(appPageUnit);

        var pages = new List<Page>
        {
            new(Guid.Parse("3a135b88-adf2-e1d5-93b1-67ba3d2d0e15"))
            {
                ApplicationId = app.Id,
                BusinessUnitId = appPageUnit.Id,
                Name = "application",
                DisplayName = "应用管理",
                Path = "/sysMng/functionPage/application",
                Icon = "el-icon-grid",
                Sort = "10"
            },
            new(Guid.Parse("3a135b88-adf2-df99-8a0f-38955ea077b1"))
            {
                ApplicationId = app.Id,
                BusinessUnitId = appPageUnit.Id,
                Name = "page",
                DisplayName = "页面管理",
                Path = "/sysMng/functionPage/page",
                Icon = "el-icon-chrome-filled",
                Sort = "11"
            }
        };
        #endregion

        #region AIGC
        var aigcUnit = new BusinessUnit(Guid.Parse("b2d0eaab-82e5-4308-af62-6a84870bde25"))
        {
            ApplicationId = app.Id,
            Name = "aigc",
            DisplayName = "AIGC",
            Icon = "el-icon-guide",
            Sort = "10",
        };
        pages.Add(new(Guid.Parse("b7c6d75e-2b76-42b7-a219-402883d3b589"))
        {
            ApplicationId = app.Id,
            BusinessUnitId = aigcUnit.Id,
            Name = "model",
            DisplayName = "模型管理",
            Path = "/sysMng/aigc/model",
            Icon = "el-icon-film",
            Sort = "10"
        });
        pages.Add(new(Guid.Parse("dce47e58-f4be-4238-9615-16d5ec3aa2f4"))
        {
            ApplicationId = app.Id,
            BusinessUnitId = aigcUnit.Id,
            Name = "agent",
            DisplayName = "AI应用",
            Path = "/sysMng/aigc/agent",
            Icon = "el-icon-coordinate",
            Sort = "20"
        });

        businessUnits.Add(aigcUnit);
        #endregion

        return (app, businessUnits, pages);
    }
    public string GetDisplayName(string name)
    {
        LocalizableString localizable = L(name);
        if (localizable == null) return name;

        return LocalizableStringSerializer.Serialize(localizable) ?? string.Empty;
    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SysMngResource>(name);
    }
}
