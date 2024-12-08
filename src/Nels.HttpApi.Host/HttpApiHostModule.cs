using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nels.Abp.SysMng;
using Nels.Aigc;
using Nels.Aigc.EntityFrameworkCore;
using Nels.Aigc.MultiTenancy;
using Nels.Aigc.Services;
using Nels.SemanticKernel;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using Nels.SemanticKernel.Services;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Nels;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AigcApplicationModule),
    typeof(AigcEntityFrameworkCoreModule)
)]
public class HttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Aigc");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
        ConfigureAigc(context);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        // 配置日志级别
        // ConfigureLogging(context, configuration);

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidateIgnoredHttpMethods.Add("POST");
        });
        Configure<MvcNewtonsoftJsonOptions>(options =>
        {
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//对类型为DateTime的生效
        });
        Configure<AbpJsonOptions>(options =>
        {
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss"; //对类型为DateTimeOffset生效
        });

    }

    private void ConfigureLogging(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        // 设置默认日志级别
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }
    private void ConfigureAigc(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IModelInstanceService, ModelInstanceAppService>();
        context.Services.AddSingleton<IPromptService, PromptAppService>();
        context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        context.Services.AddScoped<IStreamResponse, SseStreamResponse>();

        context.Services.AddKernelProcess(option =>
        {
        });
        context.Services.AddKernelBuilder(_builder =>
        {
            _builder.Services.AddSingleton<IModelInstanceService, ModelInstanceAppService>();
            _builder.Services.AddSingleton<IPromptService, PromptAppService>();
            _builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            _builder.Services.AddScoped<IStreamResponse, SseStreamResponse>();
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AigcDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..\\40 Nels.Aigc{Path.DirectorySeparatorChar}Nels.Aigc.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<AigcDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..\\40 Nels.Aigc{Path.DirectorySeparatorChar}Nels.Aigc.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<AigcApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..\\40 Nels.Aigc{Path.DirectorySeparatorChar}Nels.Aigc.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<AigcApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..\\40 Nels.Aigc{Path.DirectorySeparatorChar}Nels.Aigc.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AigcApplicationModule).Assembly);
            options.ConventionalControllers.Create(typeof(NelsAbpSysMngApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                    {"Aigc", "Aigc API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Aigc API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en");
            options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
        });

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agent API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Aigc");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
