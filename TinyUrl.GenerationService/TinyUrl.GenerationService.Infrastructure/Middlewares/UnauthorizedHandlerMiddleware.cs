using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TinyUrl.GenerationService.Infrastructure.Common;
using TinyUrl.GenerationService.Infrastructure.Exceptions;


namespace TinyUrl.GenerationService.Infrastructure.Middlewares
{
    public class UnauthorizedHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    throw new UnauthorizedException(ErrorMessages.TokenExpiredErrorMessage);
                }
            }
            catch (UnauthorizedException ex)
            {
                var error = ObjectResultCreator.To401UnauthorizedResult(ex.Message, ErrorTitles.UnauthorizedErrorTitle);

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var jsonResponse = JsonSerializer.Serialize(error.Value);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
