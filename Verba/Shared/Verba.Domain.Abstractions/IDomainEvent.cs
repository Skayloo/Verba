using System;

namespace Verba.Domain.Abstractions;

public interface IDomainEvent
{        
    DateTime OccurredOn { get; }
}
