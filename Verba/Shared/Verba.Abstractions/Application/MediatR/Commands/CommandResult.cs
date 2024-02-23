namespace Verba.Abstractions.Application.MediatR.Commands;

public abstract class CommandResult
{
    protected CommandResult(int statusCode, string error)
    {
        StatusCode = statusCode;
        Error = error;
    }
   
    public int StatusCode { get;  }

    public string Error { get; }
}

public abstract class CommandResult<T> : CommandResult
{        
    public T Result { get; set; }

    protected CommandResult(int statusCode, string error) : base(statusCode, error)
    {            
    }

    protected CommandResult(int statusCode, string error, T result):base(statusCode, error)
    {
        Result = result;
    }

    protected CommandResult(T result) : base(200, string.Empty)
    {            
        Result = result;
    }
}