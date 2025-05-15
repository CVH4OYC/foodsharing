namespace Foodsharing.API.Interfaces.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile? imageFile, string pathFolder);
}
