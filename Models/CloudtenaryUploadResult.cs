namespace Cloudtenary.Models
{
    public record CloudtenaryUploadResult
    {
        public string Url { get; init; } = string.Empty;
        public string PublicId { get; init; } = string.Empty;
    }
}
