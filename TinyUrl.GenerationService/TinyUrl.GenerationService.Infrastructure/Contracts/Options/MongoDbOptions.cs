namespace TinyUrl.GenerationService.Infrastructure.Contracts.Options
{
    public class MongoDbOptions
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
