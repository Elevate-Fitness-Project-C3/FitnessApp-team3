using FluentValidation;
using MediatR;
using NutritionService.Comman.Results;

namespace NutritionService.Comman.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResult>
   : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;
        public async Task<TResult> Handle(
            TRequest request,
            RequestHandlerDelegate<TResult> next,
            CancellationToken ct)
        {
            if (!_validators.Any())
                return await next();
            var context = new ValidationContext<TRequest>(request);
            var validateResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, ct)));
            var firstFailure = validateResults.SelectMany(r => r.Errors)
                           .FirstOrDefault(e => e is not null);
            if (firstFailure is null)
                return await next();
            var validationError = Error.Validation(firstFailure.PropertyName, firstFailure.ErrorMessage);
            return CreateFailureResult(validationError);
        }
        private static TResult CreateFailureResult(Error error)
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (TResult)Result.Failure(error);
            }

            var innerValueType = typeof(TResult)
                .GetGenericArguments()[0];

            var failureMethod = typeof(Result)
                .GetMethods()
                .First(m => m.Name == nameof(Result.Failure)
                         && m.IsGenericMethod)
                .MakeGenericMethod(innerValueType);

            return (TResult)failureMethod.Invoke(
                null,
                new object[] { error })!;
        }
    }

}
