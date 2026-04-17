using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Models;
using AutoMapper;

namespace TaskManagement.Api.Services;

public interface IStorageService : ICreateService<MediaItem, MediaDto, CreateMediaDto>, IReadService<MediaItem, MediaDto>, IDeleteService<MediaItem>
{
    Task<MediaItem> UploadFileAsync(IFormFile file);
    Task DeleteFileAsync(string fileUrl);
}

public class LocalFileStorageService : CrudService<MediaItem, MediaDto, CreateMediaDto, MediaDto>, IStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;
    private const string UploadsDirectory = "uploads";

    public LocalFileStorageService(
        ICrudRepository<MediaItem> repository,
        IMapper mapper,
        IWebHostEnvironment environment, 
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService) : base(repository, mapper)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public async Task<MediaItem> UploadFileAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, UploadsDirectory);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var request = _httpContextAccessor.HttpContext!.Request;
        var fileUrl = $"{request.Scheme}://{request.Host}/{UploadsDirectory}/{fileName}";

        return new MediaItem
        {
            FileName = file.FileName,
            FileUrl = fileUrl,
            ContentType = file.ContentType,
            Size = file.Length,
            CreatorId = _currentUserService.UserId!
        };
    }

    public Task DeleteFileAsync(string fileUrl)
    {
        try 
        {
            var uri = new Uri(fileUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            var filePath = Path.Combine(_environment.WebRootPath, UploadsDirectory, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch 
        {
            // Ignore malformed URLs in delete
        }
        return Task.CompletedTask;
    }

    public override async Task<MediaDto> CreateAsync(CreateMediaDto dto)
    {
        var mediaItem = await UploadFileAsync(dto.File);
        await _repository.AddAsync(mediaItem);
        return _mapper.Map<MediaDto>(mediaItem);
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) return false;

        await DeleteFileAsync(entity.FileUrl);
        await _repository.DeleteAsync(id);
        return true;
    }
}
