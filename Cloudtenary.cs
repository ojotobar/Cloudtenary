using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Cloudtenary.Models;
using Cloudtenary.Settings;
using Microsoft.Extensions.Options;

namespace Cloudtenary
{
    public class Cloudtenary : ICloudtenary
    {
        private readonly Cloudinary _cloud;

        public Cloudtenary(IOptions<CloudtenarySettings>? settings)
        {
            if(settings?.Value == null) throw new ArgumentNullException(nameof(settings));

            var cloudAccount = new Account
            {
                ApiKey = settings.Value.Key,
                ApiSecret = settings.Value.Secret,
                Cloud = settings.Value.CloudName
            };
            _cloud = new Cloudinary(cloudAccount);
        }

        /// <summary>
        /// Upload image file to cloudinary with height and width transformations
        /// </summary>
        /// <param name="fileName">Given file name</param>
        /// <param name="stream">File stream</param>
        /// <param name="width">Desired width of the file</param>
        /// <param name="height">Desired height of the file</param>
        /// <returns>An object with the public id and the url</returns>
        public async Task<CloudtenaryUploadResult?> UploadImage(string fileName, Stream stream, int width, int height)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().Width(width).Height(height)
            };
            var res = await _cloud.UploadAsync(imageUploadParams);
            return res.StatusCode != System.Net.HttpStatusCode.OK ? 
                null : 
                new CloudtenaryUploadResult { PublicId = res.PublicId, Url = res.Url.ToString() };
        }

        /// <summary>
        /// Upload image file to cloudinary without transformations
        /// </summary>
        /// <param name="fileName">Given file name</param>
        /// <param name="stream">File stream</param>
        /// <returns>An object with the public id and the url</returns>
        public async Task<CloudtenaryUploadResult?> UploadImage(string fileName, Stream stream)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation()
            };
            var res = await _cloud.UploadAsync(imageUploadParams);
            return res.StatusCode != System.Net.HttpStatusCode.OK ?
                null :
                new CloudtenaryUploadResult { PublicId = res.PublicId, Url = res.Url.ToString() };
        }

        /// <summary>
        /// Delete media file from cloudinary
        /// </summary>
        /// <param name="id">the public id of the resource to be deleted</param>
        /// <param name="type">The resource type of the file to be deleted</param>
        /// <returns>True/False</returns>
        public async Task<bool> DeleteMediaFile(string id, ResourceType type)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var deletionParams = new DeletionParams(id)
            {
                ResourceType = type
            };
            var delRes = await _cloud.DestroyAsync(deletionParams);
            return delRes.StatusCode == System.Net.HttpStatusCode.OK && delRes.Result.ToLower() == "ok";
        }
    }
}
