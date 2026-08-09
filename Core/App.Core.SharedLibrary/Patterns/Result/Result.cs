namespace App.Core.SharedLibrary.Patterns.Result
{

public class Result : ResultCommonLogic
{
    private Result()
        : base(ResultType.Ok, isFailure: false, string.Empty)
    {
    }

    internal Result(ResultType resultType, string message)
        : base(resultType, isFailure: true, message)
    {
    }

    public static Result Ok()
    {
        return new Result();
    }

    public static Result Conflicted(string message)
    {
        return new Result(ResultType.Conflicted, message);
    }

    public static Result Failed(string message)
    {
        return new Result(ResultType.InternalError, message);
    }

    public static Result Forbidden(string message)
    {
        return new Result(ResultType.Forbidden, message);
    }

    public static Result Invalid(string message)
    {
        return new Result(ResultType.Invalid, message);
    }

    public static Result NotFound(string message)
    {
        return new Result(ResultType.NotFound, message);
    }

    public static Result Unauthorized(string message)
    {
        return new Result(ResultType.Unauthorized, message);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Conflicted<T>(string message)
    {
        return new Result<T>(ResultType.Conflicted, message);
    }

    public static Result<T> Failed<T>(string message)
    {
        return new Result<T>(ResultType.InternalError, message);
    }

    public static Result<T> Forbidden<T>(string message)
    {
        return new Result<T>(ResultType.Forbidden, message);
    }

    public static Result<T> Invalid<T>(string message)
    {
        return new Result<T>(ResultType.Invalid, message);
    }

    public static Result<T> NotFound<T>(string message)
    {
        return new Result<T>(ResultType.NotFound, message);
    }

    public static Result<T> Unauthorized<T>(string message)
    {
        return new Result<T>(ResultType.Unauthorized, message);
    }

    public static Result<T> FromError<T>(ResultCommonLogic result)
    {
        return new Result<T>(result.ResultType, result.Message);
    }

    public static Result FromError(ResultCommonLogic result)
    {
        return new Result(result.ResultType, result.Message);
    }

    public static Result FirstFailureOrOk(params Result[] results)
    {
        if (results.Any((Result f) => f.IsFailure))
        {
            return results.First((Result f) => f.IsFailure);
        }

        return Ok();
    }

    public static Result FirstFailureOrOk<T>(IEnumerable<Result<T>> results)
    {
        if (results.Any((Result<T> x) => x.IsFailure))
        {
            return results.First((Result<T> x) => x.IsFailure);
        }

        return Ok();
    }
}
}