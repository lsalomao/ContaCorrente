using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;

namespace Led.ContaCorrente.Api.Common
{
    [ExcludeFromCodeCoverage]
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<JsonOptions> _jsonOptions;

        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptions)
        {
            _next = next;
            _jsonOptions = jsonOptions;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment hostingEnvironment, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, hostingEnvironment, logger, DateTime.Now);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, IWebHostEnvironment hostingEnvironment, ILogger<ErrorHandlingMiddleware> logger, DateTime dtNow)
        {
            logger.LogError(ex, "log-{dtNow}", dtNow);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(new 
            {
                DetalheErro = hostingEnvironment.IsProduction() ? "Ocorreu um erro interno ao processar os dados" : ex.Message
            }, _jsonOptions.Value.JsonSerializerOptions));
        }
    }
}
