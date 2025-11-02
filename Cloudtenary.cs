using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Cloudtenary.Abstract;
using Cloudtenary.Models;
using Cloudtenary.Settings;
using System.Net;

namespace Cloudtenary
{
    public class Cloudtenary : ICloudtenary
    {
        private readonly Cloudinary _cloud;

        public Cloudtenary(CloudtenarySettings settings)
        {
            var cloudAccount = new Account
            {
                ApiKey = settings.Key,
                ApiSecret = settings.Secret,
                Cloud = settings.CloudName
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
        public async Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName,
                                                                     string originalFileName,
                                                                     Stream stream,
                                                                     int width = 320,
                                                                     int height = 320,
                                                                     string overlayText = "")
        {
            var transformation = new Transformation()
                .Width(width)
                .Height(height)
                .Crop("limit")
                .Gravity("auto")
                .Quality("auto");

            if (!string.IsNullOrWhiteSpace(overlayText))
            {
                transformation = transformation
                    .Overlay(new TextLayer()
                        .FontFamily("Arial")
                        .FontSize(GetOverlaySize(width))
                        .Text(overlayText))
                        .Gravity("south_east")
                        .Opacity(60)
                        .X(10)
                        .Y(10);
            }

            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(originalFileName, stream),
                Folder = "images",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                UseFilename = false,
                UniqueFilename = false,
                Transformation = transformation
            };

            var uploadResult = await _cloud.UploadAsync(imageUploadParams);
            if (uploadResult == null || uploadResult.StatusCode != HttpStatusCode.OK)
                return null;

            return new CloudtenaryUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        /// <summary>
        /// Upload image file to cloudinary without transformations
        /// </summary>
        /// <param name="fileName">Given file name</param>
        /// <param name="stream">File stream</param>
        /// <returns>An object with the public id and the url</returns>
        public async Task<CloudtenaryUploadResult?> UploadImageAsync(string fileName,
                                                                     Stream stream)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation()
            };

            var uploadResult = await _cloud.UploadAsync(imageUploadParams);
            if (uploadResult == null || uploadResult.StatusCode != HttpStatusCode.OK)
                return null;

            return new CloudtenaryUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        /// <summary>
        /// Uploads a video stream to Cloudinary with optional transformation parameters,
        /// including resizing, trimming, fade effects, and text overlay.
        /// </summary>
        /// <param name="fileName">The original file name of the video being uploaded.</param>
        /// <param name="stream">The video stream to upload.</param>
        /// <param name="width">The target width of the video (default: 720px).</param>
        /// <param name="height">The target height of the video (default: 480px).</param>
        /// <param name="startOffset">The number of seconds to skip from the start of the video (default: 3s).</param>
        /// <param name="duration">The duration of the trimmed video in seconds (default: 60s).</param>
        /// <param name="overlayText">Optional text to overlay on the video (default: none).</param>
        /// <returns>
        /// A <see cref="CloudtenaryUploadResult"/> containing the Cloudinary public ID and
        /// secure URL of the uploaded video, or <c>null</c> if the upload fails.
        /// </returns>
        /// <remarks>
        /// This method applies the specified transformations on upload and converts the
        /// final video to MP4 format. If a video with the same public ID already exists,
        /// it will not be overwritten.
        /// </remarks>
        public async Task<CloudtenaryUploadResult?> UploadVideoAsync(string fileName, 
                                                                     Stream stream,
                                                                     int width = 720,
                                                                     int height = 480,
                                                                     int startOffset = 2,
                                                                     int duration = 120,
                                                                     string overlayText = "")
        {
            var transformation = new Transformation()
                .Width(width)
                .Height(height)
                .Crop("limit")
                .Gravity("auto")
                .Quality("auto")
                .StartOffset(startOffset)
                .Duration(duration)
                .Effect("fade:1000,fadeout:1000");

            if (!string.IsNullOrWhiteSpace(overlayText))
            {
                transformation = transformation
                    .Overlay(new TextLayer()
                        .FontFamily("Arial")
                        .FontSize(GetOverlaySize(width))
                        .Text(overlayText))
                        .Gravity("south_east")
                        .Opacity(60)
                        .X(10)
                        .Y(10);
            }

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = "videos",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = transformation
                    .FetchFormat("mp4")
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
        /// Uploads a raw video stream to Cloudinary without applying any transformations.
        /// </summary>
        /// <param name="fileName">The original file name of the video being uploaded.</param>
        /// <param name="stream">The video stream to upload.</param>
        /// <returns>
        /// A <see cref="CloudtenaryUploadResult"/> containing the Cloudinary public ID and
        /// secure URL of the uploaded video, or <c>null</c> if the upload fails.
        /// </returns>
        /// <remarks>
        /// This simplified overload performs a direct upload using the default Cloudinary
        /// configuration. It’s best suited for short or already processed video files.
        /// </remarks>
        public async Task<CloudtenaryUploadResult?> UploadVideoAsync(string fileName,
                                                                     Stream stream)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = "videos",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
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
        /// Uploads a raw document to cloudinary.
        /// fileName argument is the desired file name
        /// originalFileName is the uploaded original file name
        /// stream if the file stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="originalFileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<CloudtenaryUploadResult?> UploadRawFileAsync(string fileName,
                                                                       string originalFileName,
                                                                       Stream fileStream)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(originalFileName, fileStream),
                Folder = "documents",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                UseFilename = false,
                UniqueFilename = true,
                AccessMode = "public"
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
        public async Task<bool> DeleteMediaFileAsync(string id,
                                                     ResourceType type)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var deletionParams = new DeletionParams(id)
            {
                ResourceType = type
            };

            var deletionResult = await _cloud.DestroyAsync(deletionParams);
            if (deletionResult == null || deletionResult.StatusCode != HttpStatusCode.OK)
                return false;

            return deletionResult.Result.ToLower() == "ok";
        }

        private int GetOverlaySize(int width)
        {
            return width switch
            {
                <= 320 => 10,
                <= 640 => 14,
                _ => 18
            };
        }
    }
}
