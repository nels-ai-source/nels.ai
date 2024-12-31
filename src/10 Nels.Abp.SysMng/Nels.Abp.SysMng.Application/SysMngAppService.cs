using Microsoft.AspNetCore.Mvc;
using Nels.Abp.SysMng.Localization;
using System.IO;
using Volo.Abp.Application.Services;

namespace Nels.Abp.SysMng;

public abstract class SysMngAppService : ApplicationService
{
    protected SysMngAppService()
    {
        LocalizationResource = typeof(SysMngResource);
        ObjectMapperContext = typeof(NelsAbpSysMngApplicationModule);
    }

    public static IActionResult File(Stream fileStream, string contentType)
    {
        return new FileStreamResult(fileStream, contentType);
    }
    public static IActionResult File(Stream fileStream, string contentType, string fileDownloadName)
    {
        return new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = fileDownloadName
        };
    }

    public static IActionResult File(byte[] fileContent, string contentType, string fileDownloadName)
    {
        return new FileContentResult(fileContent, contentType)
        {
            FileDownloadName = fileDownloadName
        };
    }

    public static IActionResult File(string physicalPath, string contentType, string fileDownloadName)
    {
        return new PhysicalFileResult(physicalPath, contentType)
        {
            FileDownloadName = fileDownloadName
        };
    }
}
