using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Led.ContaCorrente.Api.SwaggerExtensions
{
    public static class ConfigurarSwagger
    {
        /// <summary>
        /// Configuração do swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.ExampleFilters();

                foreach (var name in Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.XML", new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }))
                    options.IncludeXmlComments(filePath: name);

                options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Cabeçalho de autorização padrão usando o esquema de Bearer (JWT). Exemplo: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.SchemaFilter<ReadOnlyFilter>();
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }
    }
}
