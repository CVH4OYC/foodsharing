using Foodsharing.API.Constants;
using Foodsharing.API.Interfaces.Services;

namespace Foodsharing.API.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null)
            return null;

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var savePath = Path.Combine(_env.WebRootPath, PathsConsts.FilesFolder, folder);
        Directory.CreateDirectory(savePath);

        var filePath = Path.Combine(savePath, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{folder}/{fileName}".Replace("\\", "/");
    }
}
