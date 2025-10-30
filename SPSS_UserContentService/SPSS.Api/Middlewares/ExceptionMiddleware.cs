
using Microsoft.AspNetCore.Http;
using SPSS.Shared.Errors;
using SPSS.Shared.Exceptions;
using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Text.Json;

namespace  SPSS.Api.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
				if (!context.Response.HasStarted)
				{
					await HandleStatusCodeAsync(context);
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unhandled exception occurred");
				
				if (!context.Response.HasStarted)
				{
					await HandleExceptionAsync(context, ex);
				}
				else
				{
					
					_logger.LogError(ex, "Cannot handle exception - response has already started");
					throw; 
				}
			}
		}

		private static async Task HandleStatusCodeAsync(HttpContext context)
		{
			var statusCode = context.Response.StatusCode;

			
			if (statusCode >= 400 && context.Response.ContentLength == null)
			{
				context.Response.ContentType = "application/json";

				var error = new Error();

				switch (statusCode)
				{
					case 401:
						error.StatusCode = "401";
						error.Message = "Unauthorized: You need to login to access this resource.";
						break;
					case 403:
						error.StatusCode = "403";
						error.Message = "Forbidden: You don't have permission to access this resource.";
						break;
					case 404:
						error.StatusCode = "404";
						error.Message = "Not Found: The requested resource was not found.";
						break;
					default:
						return; 
				}

				var jsonResponse = error.ToString();
				await context.Response.WriteAsync(jsonResponse);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			var error = new Error();

			switch (exception)
			{
                case SecurityException:
                case AuthenticationException:
					error.StatusCode = ((int)HttpStatusCode.Unauthorized).ToString();
					error.Message = exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;

				case ValidationException:
				case ArgumentNullException:
				case BadRequestException:
				case ArgumentException:
				case FormatException:
					error.StatusCode = ((int)HttpStatusCode.BadRequest).ToString();
					error.Message = exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					break;

				case NotFoundException:
				case FileNotFoundException:
				case KeyNotFoundException:
					error.StatusCode = ((int)HttpStatusCode.NotFound).ToString();
					error.Message = exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					break;

				case BusinessRuleException:
				case InvalidOperationException:
					error.StatusCode = ((int)HttpStatusCode.Conflict).ToString();
					error.Message = exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.Conflict;
					break;

				case UnauthorizedAccessException:
					error.StatusCode = ((int)HttpStatusCode.Unauthorized).ToString();
					error.Message = exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					break;

				case TimeoutException:
					error.StatusCode = ((int)HttpStatusCode.RequestTimeout).ToString();
					error.Message = "Request Timeout: " + exception.Message;
					context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
					break;

				default:
					error.StatusCode = ((int)HttpStatusCode.InternalServerError).ToString();
					error.Message = "Internal Server Error: An unexpected error occurred.";
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}

			var jsonResponse = error.ToString();
			await context.Response.WriteAsync(jsonResponse);
		}
	}
}
