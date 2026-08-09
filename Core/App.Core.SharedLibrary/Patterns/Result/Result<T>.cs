namespace App.Core.SharedLibrary.Patterns.Result
{
    public class Result<T> : ResultCommonLogic
    {
        public bool IsEmpty
        {
            get
            {
                T value = Value;
                if (value == null)
                {
                    return true;
                }

                object obj = Empty!;
                return value.Equals(obj);
            }
        }

        public T Value { get; }

        private static T Empty => default(T)!;

        internal Result(ResultType resultType, string message)
            : base(resultType, isFailure: true, message)
        {
            Value = Empty;
        }

        internal Result(T value)
            : base(ResultType.Ok, isFailure: false, string.Empty)
        {
            Value = value;
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Result.Ok();
            }

            return new Result(result.ResultType, result.Message);
        }
    }
}