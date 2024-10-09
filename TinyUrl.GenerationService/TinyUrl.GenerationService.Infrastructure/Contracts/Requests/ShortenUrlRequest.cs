namespace TinyUrl.GenerationService.Infrastructure.Contracts.Requests
{
    public class ShortenUrlRequest
    {
        public string? LongUrl { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int UserId { get; set; }
    }
}
