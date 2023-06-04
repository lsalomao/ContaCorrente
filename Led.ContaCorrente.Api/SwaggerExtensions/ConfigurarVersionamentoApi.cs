using Microsoft.AspNetCore.Mvc;

namespace Led.ContaCorrente.Api.SwaggerExtensions
{
    public static class ConfigurarVersionamentoApi
    {
        /// <summary>
        /// Configure as propriedades de versão da API, como cabeçalhos de retorno, formato de versão etc
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddConfiguracaoVersionamento(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // ReportApiVersions retornará os cabeçalhos "api-supported-versions" e "api-deprecated-versions".
                options.ReportApiVersions = true;

                // Defina uma versão padrão quando não for fornecida,
                // por exemplo, para compatibilidade com versões anteriores ao aplicar o controle de versão em APIs existentes
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Suporte ao versionamento na documentação.
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
