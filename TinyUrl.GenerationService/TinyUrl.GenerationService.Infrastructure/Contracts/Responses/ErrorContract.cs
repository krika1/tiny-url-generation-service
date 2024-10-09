namespace TinyUrl.GenerationService.Infrastructure.Contracts.Responses
{
    public class ErrorContract
    {
        public int StatusCode { get; set; }
        public string? Deatils { get; set; }
        public string? Title { get; set; }

        public ErrorContract(int statusCode, string? details, string? title)
        {
            StatusCode = statusCode;
            Deatils = details;
            Title = title;
        }
    }
}
