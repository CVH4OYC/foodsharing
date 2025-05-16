namespace Foodsharing.API.DTOs.Parthner;

public class AcceptApplicationRequest
{
    public Guid applicationId {  get; set; }

    public string? Comment { get; set; }
}
