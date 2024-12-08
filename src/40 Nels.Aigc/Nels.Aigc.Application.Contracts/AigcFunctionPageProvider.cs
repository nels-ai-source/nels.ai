using Nels.Abp.SysMng.FunctionPage;
using Nels.Abp.SysMng.Localization;
using Nels.Aigc.Localization;
using System;
using System.Collections.Generic;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace Nels.Aigc;

public class AigcFunctionPageProvider(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer) : FunctionPageProvider
{
    public IGuidGenerator GuidGenerator = guidGenerator;
    public ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public override void Define(IFunctionPageDefinitionContext context)
    {
        var (app, businessUnits, pages) = InitSysMngApp();
        context.AddGroup(app, businessUnits, pages);
    }

    private (App app, List<BusinessUnit> businessUnits, List<Page> pages) InitSysMngApp()
    {
        var appId = Guid.Parse("e9b7d16c-74ef-4a33-be34-e59889e76820");
        var app = new App(appId)
        {
            Name = "aigc",
            DisplayName = "Menu:Aigc",
            Icon = "el-icon-magic-stick",
            Sort = "40"
        };
        var agentUnitId = Guid.Parse("13bf32ef-9e9e-4573-a319-7e17d5eadaa5");
        var resourceUnitId = Guid.Parse("5984dec5-60ed-4c66-9fa5-40851ed20457");
        var settingUnitId = Guid.Parse("f418b43c-1335-4d4e-8c1b-05deb4014fc3");
        var businessUnits = new List<BusinessUnit>()
        {
            new(agentUnitId)
            {
                ApplicationId = app.Id,
                Name = "agent",
                DisplayName = "Menu:Aigc.Agent",
                Icon = "el-icon-grid",
                Sort = "10",
            },
            new(resourceUnitId)
            {
                ApplicationId = app.Id,
                Name = "resource",
                DisplayName = "Menu:Aigc.Resource",
                Icon = "el-icon-files",
                Sort = "20",
            },
            new(settingUnitId)
            {
                ApplicationId = app.Id,
                Name = "setting",
                DisplayName = "Menu:Aigc.Setting",
                Icon = "el-icon-setting",
                Sort = "20",
            }
        };

        var pages = new List<Page>
        {
            new(Guid.Parse("e576d087-a8cf-4f7b-8848-c58fac12eca1"))
            {
                ApplicationId = appId,
                BusinessUnitId = agentUnitId,
                Name = "agent",
                DisplayName = "Menu:Aigc.Agent.Agent",
                Path = "/aigc/agent/agent",
                Icon = "el-icon-memo",
                Sort = "10"
            },
            new(Guid.Parse("a1838115-2b23-45f0-acf4-1a14a7b893f2"))
            {
                ApplicationId = appId,
                BusinessUnitId = agentUnitId,
                Name = "agentDetail",
                DisplayName = "Menu:Aigc.Agent.AgentDetail",
                Path = "/aigc/agent/agent/detail",
                Icon = "el-icon-view",
                Hidden =true,
                Fullpage=true,
                Sort = "01"
            },
            new(Guid.Parse("d85d03e9-cea5-46cc-b73f-43f41b7eb140"))
            {
                ApplicationId = appId,
                BusinessUnitId = resourceUnitId,
                Name = "knowledge",
                DisplayName = "Menu:Aigc.Resource.Knowledge",
                Path = "/aigc/resource/knowledge",
                Icon = "el-icon-coin",
                Sort = "10"
            },
            new(Guid.Parse("5b719bd9-145d-4cd2-903b-174f766e86ce"))
            {
                ApplicationId = appId,
                BusinessUnitId = resourceUnitId,
                Name = "plugin",
                DisplayName = "Menu:Aigc.Resource.Plugin",
                Path = "/aigc/resource/plugin",
                Icon = "el-icon-set-up",
                Sort = "20"
            },
            new(Guid.Parse("a1c89a2e-f661-4ef2-8d8d-acef4b5d3ae1"))
            {
                ApplicationId = appId,
                BusinessUnitId = settingUnitId,
                Name = "space",
                DisplayName = "Menu:Aigc.Setting.Space",
                Path = "/aigc/setting/space",
                Icon = "el-icon-operation",
                Sort = "10"
            },
            new(Guid.Parse("25f50af2-4eca-43a4-bc6b-60b59e90dcda"))
            {
                ApplicationId = appId,
                BusinessUnitId = settingUnitId,
                Name = "model",
                DisplayName = "Menu:Aigc.Setting.Model",
                Path = "/aigc/setting/model",
                Icon = "el-icon-film",
                Sort = "10"
            }
        };
        return (app, businessUnits, pages);
    }
}
