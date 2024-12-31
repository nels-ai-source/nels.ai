using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.SysMng.Files;

[Route(SysMngRemoteServiceConsts.fileRoute)]
public class FileAppService : SysMngAppService
{
    private readonly IBlobContainer _blobContainer;
    private readonly IRepository<FileEntity, Guid> _repository;
    public FileAppService(IBlobContainer blobContainer, IRepository<FileEntity, Guid> repository)
    {
        _blobContainer = blobContainer;
        _repository = repository;
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<FileDto> UploadFileAsync(IFormFile file)
    {
        var suffix = Path.GetExtension(file.FileName);

        var patch = $"{DateTime.UtcNow:yyyyMM}/{Guid.NewGuid()}{suffix}";

        await _blobContainer.SaveAsync(patch, file.OpenReadStream());

        var entity = new FileEntity(GuidGenerator.Create())
        {
            Name = file.FileName,
            Type = suffix,
            Size = file.Length,
            Path = patch,
        };
        await _repository.InsertAsync(entity);

        return ObjectMapper.Map<FileEntity, FileDto>(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> DownloadFileAsync(Guid id)
    {
        var file = await _repository.GetAsync(id) ?? throw new Exception();

        var stream = await _blobContainer.GetAsync(file.Path);
        var provider = new FileExtensionContentTypeProvider();
        var suffix = Path.GetExtension(file.Path);
        var contentType = provider.Mappings[suffix];
        return File(stream, "application/octet-stream", file.Name);
    }

    [HttpGet("preview/{id}")]
    public async Task<IActionResult> PreviewAsync(Guid id)
    {
        var file = await _repository.GetAsync(id) ?? throw new Exception();
        var stream = await _blobContainer.GetAsync(file.Path);

        if (file.Name.EndsWith(".pdf"))
        {
            return File(stream, "application/pdf");
        }
        else if (file.Name.EndsWith(".jpg") || file.Name.EndsWith(".png"))
        {
            return File(stream, $"image/{Path.GetExtension(file.Name).TrimStart('.')}");
        }

        return File(stream, "application/octet-stream", file.Name);
    }
}
