using Foodsharing.API.Constants;
using Foodsharing.API.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Foodsharing.API.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(IFormFile? imageFile, string pathFolder)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        var uploadsFolder = Path.Combine(_env.WebRootPath, PathsConsts.PicturesFolder, pathFolder);
        Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        using var image = await Image.LoadAsync(imageFile.OpenReadStream());

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(1280, 720)
        }));

        var ext = Path.GetExtension(imageFile.FileName).ToLower();

        if (ext == ".png")
        {
            await image.SaveAsPngAsync(filePath, new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression
            });
        }
        else
        {
            await image.SaveAsJpegAsync(filePath, new JpegEncoder
            {
                Quality = 75
            });
        }

        return $"/{pathFolder}/{fileName}".Replace("\\", "/");
    }
}
