using CloudinaryDotNet.Actions;
using Cloudtenary.Models;

namespace Cloudtenary
{
    public interface ICloudtenary
    {
        Task<CloudtenaryUploadResult?> UploadImage(string fileName, Stream stream, int width, int height);
        Task<CloudtenaryUploadResult?> UploadImage(string fileName, Stream stream);
        Task<bool> DeleteMediaFile(string id, ResourceType type);
    }
}
