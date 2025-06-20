using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Cloudtenary.Models;
using Cloudtenary.Settings;
using Microsoft.Extensions.Options;
using System.Net;

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
        /// Uploads an image to cloudinary.
        /// fileName argument is the desired file name
        /// originalFileName is the uploaded original file name
        /// stream if the file stream
        /// width is the desired image width
        /// height is the desired image height
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="originalFileName"></param>
        /// <param name="stream"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public async Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, string originalFileName, Stream stream, int width, int height)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(originalFileName, stream),
                PublicId = $"images/{fileName}",
                UseFilename = false,
                UniqueFilename = false,
                Transformation = new Transformation()
                    .Width(width)
                    .Height(height)
                    .Crop("fill")
                    .Gravity("auto")
                    .Quality("auto")
            };

            var res = await _cloud.UploadAsync(imageUploadParams);
            return res.StatusCode != System.Net.HttpStatusCode.OK ? 
                null : 
                new CloudtenaryUploadResult { PublicId = res.PublicId, Url = res.SecureUrl.ToString() };
        }

        /// <summary>
        /// Upload image file to cloudinary without transformations
        /// </summary>
        /// <param name="fileName">Given file name</param>
        /// <param name="stream">File stream</param>
        /// <returns>An object with the public id and the url</returns>
        public async Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName, Stream stream)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation()
            };
            var res = await _cloud.UploadAsync(imageUploadParams);
            return res.StatusCode != System.Net.HttpStatusCode.OK ?
                null :
                new CloudtenaryUploadResult { PublicId = res.PublicId, Url = res.SecureUrl.ToString() };
        }

        /// <summary>
        /// Uploads a raw document to cloudinary.
        /// fileName argument is the desired file name
        /// originalFileName is the uploaded original file name
        /// stream if the file stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="originalFileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<CloudtenaryUploadResult?> UploadRawFileAsync(string fileName, string originalFileName, Stream fileStream)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(originalFileName, fileStream),
                PublicId = $"documents/{fileName}",
                UseFilename = false,
                UniqueFilename = false
            };

            var uploadResult = await _cloud.UploadAsync(uploadParams);

            if (uploadResult == null || uploadResult.StatusCode != HttpStatusCode.OK)
                return null;

            return new CloudtenaryUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        /// <summary>
        /// Delete media file from cloudinary
        /// </summary>
        /// <param name="id">the public id of the resource to be deleted</param>
        /// <param name="type">The resource type of the file to be deleted</param>
        /// <returns>True/False</returns>
        public async Task<bool> DeleteMediaFileAsync(string id, ResourceType type)
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
