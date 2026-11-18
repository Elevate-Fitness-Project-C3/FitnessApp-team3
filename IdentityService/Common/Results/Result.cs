namespace IdentityService.Common.Results;

public class Result(bool isSuccess, string errorMessage)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public string ErrorMessage { get; } = errorMessage;

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}

public class Result<T>(bool isSuccess, string errorMessage, T? value = default)
    : Result(isSuccess, errorMessage)
{
    public T? Value { get; } = value;

    public static Result<T> Success(T value) => new(true, string.Empty, value);
    public new static Result<T> Failure(string error) => new(false, error);
}
