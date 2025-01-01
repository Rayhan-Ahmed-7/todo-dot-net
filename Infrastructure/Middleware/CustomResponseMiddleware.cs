using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Application.Models;

namespace TodoApp.Infrastructure.Middleware
{
    public class CustomResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            var contentType = context.Response.ContentType;

            if (contentType != null && contentType.Contains("application/json"))
            {
                var statusCode = context.Response.StatusCode;
                object responseBody;

                if (statusCode >= 400)
                {
                    // Handle error responses
                    var errors = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
                    responseBody = new ApiResponse<object>(
                        message: GetErrorMessage(errors),
                        status: statusCode,
                        data: null,
                        errors: errors
                    );
                }
                else
                {
                    // Normal response
                    var responseData = JsonSerializer.Deserialize<object>(body);

                    // Return response without wrapping in another ApiResponse
                    responseBody = new ApiResponse<object>(
                        message: "Success",
                        status: statusCode,
                        data: responseData, // Directly assigning the response data
                        errors: null
                    );
                }

                // Set the response back to the original stream and write the custom response
                context.Response.ContentType = "application/json";
                context.Response.Body = originalBodyStream;

                var json = JsonSerializer.Serialize(responseBody);
                await context.Response.WriteAsync(json);
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }

        private string GetErrorMessage(Dictionary<string, object> errors)
        {
            // Customize how you want to extract the error message
            var firstError = errors.FirstOrDefault();
            if (firstError.Key != null && firstError.Value is IEnumerable<string> messages)
            {
                foreach (var message in messages)
                {
                    if (message.Contains("is required", StringComparison.OrdinalIgnoreCase))
                    {
                        return $"{firstError.Key} | {message}";
                    }
                }
            }
            return "An error occurred";
        }
    }
}
