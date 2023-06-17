using Led.ContaCorrente.Domain.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace Led.ContaCorrente.Domain.Atributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private static string GetMessage(string key, ModelError error)
            => string.IsNullOrWhiteSpace(key) ? error.ErrorMessage : $"{key}: {error.ErrorMessage}";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var sb = new StringBuilder();

            foreach (var item in context.ModelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid))
                foreach (var error in item.Value.Errors)
                    sb.Append($" | {GetMessage(item.Key, error)}");

            context.Result = new BadRequestObjectResult(new Notificacao() { DetalheErro = sb.ToString().TrimStart(' ').TrimStart('|') });
        }
    }
}
