namespace App.Core.SharedLibrary.Patterns.Result;

public enum ResultType : short
{
    InternalError,
    Ok,
    NotFound,
    Forbidden,
    Conflicted,
    Invalid,
    Unauthorized
}
