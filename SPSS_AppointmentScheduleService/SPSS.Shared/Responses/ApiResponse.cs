using System.Collections.Generic;
using System.Linq;

namespace SPSS.Shared.Responses;

public class ApiResponse
{
	public bool Success { get; init; }
	public string? Message { get; init; }
	public IReadOnlyCollection<string>? Errors { get; init; }

	protected ApiResponse() { }

	public static ApiResponse<T> Ok<T>(T data, string message = "Thao tác thành công")
	{
		return new ApiResponse<T>
		{
			Success = true,
			Data = data,
			Message = message,
			Errors = null
		};
	}

	public static ApiResponse Fail(string message, IEnumerable<string>? errors = null)
	{
		return new ApiResponse
		{
			Success = false,
			Message = message,
			Errors = errors?.ToList().AsReadOnly()
		};
	}

	public static ApiResponse<T> Fail<T>(string message, IEnumerable<string>? errors = null)
	{
		return new ApiResponse<T>
		{
			Success = false,
			Data = default,
			Message = message,
			Errors = errors?.ToList().AsReadOnly()
		};
	}
}


public class ApiResponse<T> : ApiResponse
{
	public T? Data { get; init; }
	protected internal ApiResponse() : base() { }
}