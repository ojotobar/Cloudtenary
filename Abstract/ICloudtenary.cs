using CloudinaryDotNet.Actions;
using Cloudtenary.Models;

namespace Cloudtenary.Abstract
{
    public interface ICloudtenary
    {
        Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, string originalFileName, Stream stream, int width, int height);
        Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, Stream stream);
        Task<bool> DeleteMediaFileAsync(string id, ResourceType type);
        Task<CloudtenaryUploadResult?> UploadRawFileAsync(string fileName, string originalFileName, Stream fileStream);
        Task<CloudtenaryUploadResult?> UploadVideoAsync(string fileName, Stream stream, int width = 720, int height = 480, int startOffset = 3, int duration = 60, string overlayText = "");
        Task<CloudtenaryUploadResult?> UploadVideoAsync(string fileName, Stream stream);
    }
}