using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Verba.Abstractions.Application.MediatR.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest>[] _validators;
    public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

    //public async Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    //{
    //    var failures = _validators
    //        .Select(v => v.Validate(request))
    //        .SelectMany(result => result.Errors)
    //        .Where(error => error != null)
    //        .ToList();

    //    if (failures.Any())
    //    {
    //        throw new ValidationException("Validation exception", failures);
    //    }

    //    return await next();            
    //}

    async Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException("Validation exception", failures);
        }

        return await next();
    }
}
