using CloudinaryDotNet.Actions;
using Cloudtenary.Models;

namespace Cloudtenary
{
    public interface ICloudtenary
    {
        Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, string originalFileName, Stream stream, int width, int height);
        Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, Stream stream);
        Task<bool> DeleteMediaFileAsync(string id, ResourceType type);
        Task<CloudtenaryUploadResult?> UploadRawFileAsync(string fileName, string originalFileName, Stream fileStream);
    }
}
