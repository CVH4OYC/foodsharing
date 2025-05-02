using Foodsharing.API.Constants;
using Foodsharing.API.Interfaces;

namespace Foodsharing.API.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile, string pathFolder)
    {
        // Генерируем уникальное имя файла
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

        var uploadsFolder = Path.Combine(_env.WebRootPath, PathsConsts.PicturesFolder, pathFolder);
        Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        // Сохраняем файл
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"/{filePath}";
    }
}
