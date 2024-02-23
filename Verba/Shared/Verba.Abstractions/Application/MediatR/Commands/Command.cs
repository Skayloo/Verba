using MediatR;

namespace Verba.Abstractions.Application.MediatR.Commands;

public class Command<T> : IRequest<T> where T: CommandResult
{
}