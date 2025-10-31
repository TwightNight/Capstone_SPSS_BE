using System;
using System.Collections.Generic;
using System.Linq;

namespace SPSS.Shared.Responses;

public class ApiResponse
{
    public bool Success { get; }
    public string Message { get; }
    public IReadOnlyCollection<string>? Errors { get; }

    protected ApiResponse(bool success, string message, IEnumerable<string>? errors)
    {
        Success = success;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Errors = errors?.ToList().AsReadOnly();
    }

    public static ApiResponse<T> Ok<T>(T data, string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>(data, true, message, null);
    }

    public static ApiResponse Fail(string message, IEnumerable<string>? errors = null)
    {
        return new ApiResponse(false, message, errors);
    }

    public static ApiResponse<T> Fail<T>(string message, IEnumerable<string>? errors = null)
    {
        return new ApiResponse<T>(default, false, message, errors);
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; }

    internal ApiResponse(T? data, bool success, string message, IEnumerable<string>? errors)
       : base(success, message, errors)
    {
        Data = data;
    }
}