namespace TinyUrl.GenerationService.Infrastructure.Contracts.Responses
{
    public class UrlMappingContract
    {
        public string? ShortUrl { get; set; }
        public string? LongUrl { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
