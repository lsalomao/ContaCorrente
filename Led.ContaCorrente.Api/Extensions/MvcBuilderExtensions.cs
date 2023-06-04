using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Led.ContaCorrente.DomainService;
using System.Globalization;

namespace Led.ContaCorrente.Api.Extensions
{
    internal static class MvcBuilderExtensions
    {
        internal static IMvcBuilder AddValidators(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<DomainServiceRef>();
                fv.AutomaticValidationEnabled = true;
                fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
            });
            return builder;
        }
    }
}
