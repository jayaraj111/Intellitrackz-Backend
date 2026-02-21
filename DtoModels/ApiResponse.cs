namespace AdminDashboard.DtoModels;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse<T> Ok(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };
    }

    public static ApiResponse<T> Ok(string message)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Message = message,
            Data = default,
            Errors = new List<string>()
        };
    }

    public static ApiResponse<T> Fail(
        int statusCode,
        string message,
        List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Data = default,
            Errors = errors ?? new List<string>()
        };
    }
}
