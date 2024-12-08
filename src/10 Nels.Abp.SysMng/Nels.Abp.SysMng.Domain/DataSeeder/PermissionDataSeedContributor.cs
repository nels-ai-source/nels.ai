using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace Nels.Abp.SysMng.DataSeeder;

public class PermissionDataSeedContributor(
    IOptions<AbpPermissionOptions> options,
    IServiceProvider serviceProvider,
    IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
    IPermissionDefinitionRecordRepository permissionRepository,
    IGuidGenerator guidGenerator,
    ILocalizableStringSerializer localizableStringSerializer,
    ISimpleStateCheckerSerializer stateCheckerSerializer) : IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        var definitionContext = new PermissionDefinitionContext(serviceProvider);

        var providers = options.Value
        .DefinitionProviders
            .Select(p => serviceProvider.GetService(p) as IPermissionDefinitionProvider)
            .ToList();
        foreach (var provider in providers)
        {
            provider?.PreDefine(definitionContext);
        }

        foreach (var provider in providers)
        {
            provider?.Define(definitionContext);
        }

        foreach (var provider in providers)
        {
            provider?.PostDefine(definitionContext);
        }

        var permissionGroups = definitionContext.Groups.Select(x => x.Value).ToList();

        var (permissionGroupRecords, permissionRecords) = await SerializeAsync(permissionGroups);

        await UpdateChangedPermissionGroupsAsync(permissionGroupRecords);

        await UpdateChangedPermissionsAsync(permissionRecords);
    }

    private async Task<bool> UpdateChangedPermissionGroupsAsync(IEnumerable<PermissionGroupDefinitionRecord> permissionGroupRecords)
    {
        var newRecords = new List<PermissionGroupDefinitionRecord>();
        var changedRecords = new List<PermissionGroupDefinitionRecord>();

        var permissionGroupRecordsInDatabase = (await permissionGroupRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroupRecordInDatabase = permissionGroupRecordsInDatabase.GetOrDefault(permissionGroupRecord.Name);
            if (permissionGroupRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(permissionGroupRecord);
                continue;
            }

            if (permissionGroupRecord.HasSameData(permissionGroupRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionGroupRecordInDatabase.Patch(permissionGroupRecord);
            changedRecords.Add(permissionGroupRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = permissionGroupRecordsInDatabase.Values.Where(x => permissionGroupRecords.Any(o => x.Name == x.Name) == false).ToList();

        if (newRecords.Any())
        {
            await permissionGroupRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await permissionGroupRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await permissionGroupRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private async Task<bool> UpdateChangedPermissionsAsync(IEnumerable<PermissionDefinitionRecord> permissionRecords)
    {
        var newRecords = new List<PermissionDefinitionRecord>();
        var changedRecords = new List<PermissionDefinitionRecord>();

        var permissionRecordsInDatabase = (await permissionRepository.GetListAsync()).ToDictionary(x => x.Name);

        foreach (var permissionRecord in permissionRecords)
        {
            var permissionRecordInDatabase = permissionRecordsInDatabase.GetOrDefault(permissionRecord.Name);
            if (permissionRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(permissionRecord);
                continue;
            }

            if (permissionRecord.HasSameData(permissionRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionRecordInDatabase.Patch(permissionRecord);
            changedRecords.Add(permissionRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = permissionRecordsInDatabase.Values.Where(x => permissionRecords.Any(o => o.Name == x.Name) == false).ToList();

        if (newRecords.Any())
        {
            await permissionRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await permissionRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await permissionRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    public async Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])> SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups)
    {
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>();
        var permissionRecords = new List<PermissionDefinitionRecord>();

        foreach (var permissionGroup in permissionGroups)
        {
            permissionGroupRecords.Add(await SerializeAsync(permissionGroup));

            foreach (var permission in permissionGroup.GetPermissionsWithChildren())
            {
                permissionRecords.Add(await SerializeAsync(permission, permissionGroup));
            }
        }

        return (permissionGroupRecords.ToArray(), permissionRecords.ToArray());
    }

    public Task<PermissionGroupDefinitionRecord> SerializeAsync(PermissionGroupDefinition permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionGroupRecord = new PermissionGroupDefinitionRecord(guidGenerator.Create(),
                permissionGroup.Name,
                localizableStringSerializer.Serialize(permissionGroup.DisplayName)
            );

            foreach (var property in permissionGroup.Properties)
            {
                permissionGroupRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(permissionGroupRecord);
        }
    }

    public Task<PermissionDefinitionRecord> SerializeAsync(
        PermissionDefinition permission,
        PermissionGroupDefinition permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionRecord = new PermissionDefinitionRecord(
                guidGenerator.Create(),
                permissionGroup?.Name,
                permission.Name,
                permission.Parent?.Name,
                localizableStringSerializer.Serialize(permission.DisplayName),
                permission.IsEnabled,
                permission.MultiTenancySide,
                SerializeProviders(permission.Providers),
                SerializeStateCheckers(permission.StateCheckers)
            );

            foreach (var property in permission.Properties)
            {
                permissionRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(permissionRecord);
        }
    }

    protected virtual string? SerializeProviders(ICollection<string> providers)
    {
        return providers.Any() ? providers.JoinAsString(",") : null;
    }

    protected virtual string? SerializeStateCheckers(List<ISimpleStateChecker<PermissionDefinition>> stateCheckers)
    {
        return stateCheckerSerializer.Serialize(stateCheckers);
    }
}
