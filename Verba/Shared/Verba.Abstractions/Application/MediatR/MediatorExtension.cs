using System;
using System.Threading;
using System.Threading.Tasks;
using Verba.Abstractions.Application.MediatR.Notification;
using Verba.Domain.Abstractions;
using MediatR;

namespace FinFactory.Abstractions.Application.MediatR
{
    public static class MediatorExtension
    {
        public static Task PublishEvent(this IMediator mediator, IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var eventType = domainEvent.GetType();
            var domainEventDecorator = Activator.CreateInstance(typeof(DomainEventDecorator<>).MakeGenericType(eventType), domainEvent);

            var method = mediator.GetType().GetMethod("Publish");
            var generic = method.MakeGenericMethod(domainEventDecorator.GetType());
            return (Task)generic.Invoke(mediator, new[] { domainEventDecorator, cancellationToken });
        }
    }
}