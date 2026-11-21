
namespace NutritionService.Comman.Results
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
    }
    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;
        internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }
        public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result.");
    }

    public sealed class Result<TValue,TError> where TError : struct , Enum
    {
        public bool IsSuccess { get; }
        public TValue ? Value { get; }
        public TError ?ErrorCode { get; }
        public string ? Error { get; }
        private Result(bool isSuccess , TValue? value , TError? errorCode , string ? error )
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorCode = errorCode;
            Error = error;
        }
        public static Result<TValue, TError> Success(TValue value) 
            => new(true, value, null, null);
        public static Result<TValue, TError> Failure(TError errorCode , string error)
           => new(false, default, errorCode, error);

    }


}

