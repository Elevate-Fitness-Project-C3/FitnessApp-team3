namespace TrainingService.Dtos.Shared;


public class EndpointResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static EndpointResponse<T> FromResult(
        RequestResponse<T> result)
    {
        return new EndpointResponse<T>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Data = result.Data,
            Errors = result.Errors
        };
    }
}

public class EndpointResponse : EndpointResponse<object>
{
    public static EndpointResponse FromResult(
        RequestResponse result)
    {
        return new EndpointResponse
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Data = result.Data,
            Errors = result.Errors
        };
    }
}

