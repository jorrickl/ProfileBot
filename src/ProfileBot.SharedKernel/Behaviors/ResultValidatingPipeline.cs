using Ardalis.Result;
using FluentValidation;
using Mapster;
using MediatR;

namespace ProfileBot.SharedKernel.Behaviors
{
    public sealed class ResultValidatingPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, TResponse
        where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next(cancellationToken).ConfigureAwait(false);
            }

            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                validators.Select(validator => validator.ValidateAsync(context)));

            var errors = validationFailures
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(x => x.Adapt<ValidationError>())
                .ToList();

            if (errors.Count != 0)
            {
                return Result<TResponse>.Invalid(errors);
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }
    }
}
