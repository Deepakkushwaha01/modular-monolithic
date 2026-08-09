using System.Diagnostics;
using System.Net;

namespace App.Core.SharedLibrary.Patterns.Result
{

[DebuggerStepThrough]
public abstract class ResultCommonLogic
{
    private readonly string _message;

    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;
    public string Message => _message;
    public ResultType ResultType { get; }

    public HttpStatusCode HttpStatusCode => ResultType switch
    {
        ResultType.Ok => HttpStatusCode.OK,
        ResultType.NotFound => HttpStatusCode.NotFound,
        ResultType.Forbidden => HttpStatusCode.Forbidden,
        ResultType.Conflicted => HttpStatusCode.Conflict,
        ResultType.Invalid => HttpStatusCode.NotAcceptable,
        ResultType.Unauthorized => HttpStatusCode.Unauthorized,
        _ => HttpStatusCode.InternalServerError,
    };

    protected ResultCommonLogic(ResultType resultType, bool isFailure, string message)
    {
        if (isFailure)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message", "There must be error message for failure.");
            if (resultType == ResultType.Ok)
                throw new ArgumentException("There should be error type for failure.", "resultType");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("There should be no error message for success.", "message");
            if (resultType != ResultType.Ok)
                throw new ArgumentException("There should be no error type for success.", "resultType");
        }

        ResultType = resultType;
        IsFailure = isFailure;
        _message = message;
    }
}

}