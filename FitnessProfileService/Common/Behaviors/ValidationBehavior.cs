using FluentValidation;
using MediatR;

namespace FitnessProfileService.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = validators
                .SelectMany(v => v.Validate(context).Errors)
                .Where(e => e != null)
                .ToList();

            if (errors.Any())
                throw new ValidationException(errors);

            return await next();
        }
    }
}