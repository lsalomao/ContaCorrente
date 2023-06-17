using FluentValidation;
using FluentValidation.Results;
using Led.ContaCorrente.Domain.Abstractions.Response;
using Led.ContaCorrente.Domain.Enums;
using MediatR;

namespace Led.ContaCorrente.Domain.Abstractions.Validadores
{
    public class FailFastRequestBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
         where TRequest : IRequest<TResult>
         where TResult : class, IResponse, new()
    {
        private readonly IEnumerable<IValidator> _validators;

        public FailFastRequestBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            return failures.Any()
                ? ValidarErros(failures)
                : TryNext(next);
        }

        private static Task<TResult> TryNext(RequestHandlerDelegate<TResult> next)
        {
            try
            {
                var interResult = next().Result;
                return Task.FromResult(interResult);
            }
            catch (Exception ex)
            {
                var errorResponse = new TResult();
                errorResponse.DefinirMotivoErro(MotivoErro.InternalServerError);
                errorResponse.AddErro(ex.Message);

                return Task.FromResult(errorResponse);
            }
        }

        private static Task<TResult> ValidarErros(IEnumerable<ValidationFailure> falhas)
        {
            var respostaErro = new TResult();
            respostaErro.DefinirMotivoErro(MotivoErro.BadRequest);
            foreach (var falha in falhas)
                respostaErro.AddErro(falha.ErrorMessage);

            return Task.FromResult(respostaErro);
        }

    }
}
