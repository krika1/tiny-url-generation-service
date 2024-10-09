namespace TinyUrl.GenerationService.Infrastructure.Entities
{
    public class UrlMapping
    {
        public string? ShortUrl { get; set; }
        public string? LongUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int UserId { get; set; }
        public int Clicks { get; set; }
    }
}
