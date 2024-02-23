namespace Verba.Abstractions.Application.MediatR.InternalRequests;

public class InternalRequestResponse<T>
{
    public T Result { get; set; }

    protected InternalRequestResponse(T result)
    {
        Result = result;
    }
}
