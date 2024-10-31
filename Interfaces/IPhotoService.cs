using CloudinaryDotNet.Actions;

namespace DatingAppServer.Interfaces;

public interface IPhotoService
{
    //Both ImageUploadResult, Deletionresult are from the cloudinarydotnet library that has been added to the project via nuget.
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}