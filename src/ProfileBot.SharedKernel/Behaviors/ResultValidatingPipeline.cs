using Ardalis.Result;
using FluentValidation;
using Mapster;
using MediatR;

namespace ProfileBot.SharedKernel.Behaviors
{
    //TODO: Try to validate the request or get rid of the Result pattern
    public sealed class ResultValidatingPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : class, IResult
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
                return CreateInvalidResult(errors);
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }

        private TResponse CreateInvalidResult(IEnumerable<ValidationError> parameters)
        {
            var resultType = typeof(TResponse).GetGenericArguments().FirstOrDefault()
                          ?? throw new InvalidOperationException();

            var genericResultType = typeof(Result<>).MakeGenericType(resultType);
            var resultInvalidMethod = genericResultType.GetMethod(nameof(Result.Invalid), [parameters.GetType()]);

            var untypedResult = resultInvalidMethod?.Invoke(null, [parameters]);
            if (untypedResult is not TResponse result)
            {
                throw new InvalidOperationException();
            }

            return result;
        }

    }
}