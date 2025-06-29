using Ardalis.Result;
using FluentValidation;
using Mapster;
using MediatR;

namespace ProfileBot.SharedKernel.Behaviors
{
    public sealed class ResultValidatingPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : IResult
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
                .Select(x => x.Adapt<ValidationError>());

            if (errors.Any())
            {
                // Dynamically create a Result<T> with errors
                var resultType = typeof(TResponse).GetGenericArguments().FirstOrDefault();
                if (resultType != null)
                {
                    var invalidObject = CreateInvalidResult(resultType, errors);
                    if (invalidObject is not TResponse invalidResult)
                    {
                        throw new InvalidOperationException();
                    }
                    return invalidResult;
                }
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }
        private object? CreateInvalidResult(Type resultType, IEnumerable<ValidationError> parameters)
        {
            // Use reflection to create a Result<T>.Invalid instance
            var genericResultType = typeof(Result<>).MakeGenericType(resultType);
            var invalidMethod = genericResultType.GetMethod(nameof(Result.Invalid), [parameters.GetType()]);

            return invalidMethod?.Invoke(null, [parameters]);
        }

    }
}