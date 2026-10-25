using System.Diagnostics;
using System.Net;
using App.Core.SharedLibrary.Errors;

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
                    throw new ArgumentNullException("message", CommonErrorMessages.FailureMessageRequired);
                if (resultType == ResultType.Ok)
                    throw new ArgumentException(CommonErrorMessages.FailureTypeRequired, "resultType");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException(CommonErrorMessages.SuccessCannotHaveMessage, "message");
                if (resultType != ResultType.Ok)
                    throw new ArgumentException(CommonErrorMessages.SuccessCannotHaveErrorType, "resultType");
            }

            ResultType = resultType;
            IsFailure = isFailure;
            _message = message;
        }
    }

}
