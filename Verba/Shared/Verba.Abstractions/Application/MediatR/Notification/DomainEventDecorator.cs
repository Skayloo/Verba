using Verba.Domain.Abstractions;
using MediatR;

namespace Verba.Abstractions.Application.MediatR.Notification;

public class DomainEventDecorator<T>: INotification where T: IDomainEvent
{
    public DomainEventDecorator(T domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public T DomainEvent { get; }
}