namespace Foodsharing.API.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile? imageFile, string pathFolder);
}
