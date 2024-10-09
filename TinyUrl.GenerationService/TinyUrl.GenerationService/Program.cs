using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyUrl.GenerationService.Bussiness.Services;
using TinyUrl.GenerationService.Data.Clients;
using TinyUrl.GenerationService.Data.Repositories;
using TinyUrl.GenerationService.Infrastructure.Clients;
using TinyUrl.GenerationService.Infrastructure.Context;
using TinyUrl.GenerationService.Infrastructure.Contracts.Options;
using TinyUrl.GenerationService.Infrastructure.Repositories;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<MongoDbOptions>(
                builder.Configuration.GetSection("MongoDbOptions"));

            builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });

            builder.Services.AddScoped<MongoDbContext>();

            builder.Services.AddScoped<IUrlMappingService, UrlMappingService>();
            builder.Services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();
            builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>();

            builder.Services.Configure<UserClientOptions>(
                 builder.Configuration.GetSection("UserClientOptions"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
