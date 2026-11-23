namespace FitnessProfileService.Features.Shared;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponse() { }

    public ApiResponse(bool success, int statusCode, string message, T? data = default, IDictionary<string, string[]>? errors = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
        Timestamp = DateTime.UtcNow;
    }

    public static ApiResponse<T> Ok(T data, string message = "Success", int statusCode = 200)
        => new(true, statusCode, message, data);

    public static ApiResponse<T> Created(T data, string message = "Created successfully")
        => new(true, 201, message, data);

    public static ApiResponse<T> Fail(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null)
        => new(false, statusCode, message, default, errors);
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok(string message = "Success", int statusCode = 200)
        => new() { Success = true, StatusCode = statusCode, Message = message };

    public static new ApiResponse Fail(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null)
        => new() { Success = false, StatusCode = statusCode, Message = message, Errors = errors };
}
