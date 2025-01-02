using System.Text.Json;
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

                // Avoid double-wrapping
                if (body.StartsWith("{") && body.Contains("\"message\"") && body.Contains("\"status\"") && body.Contains("\"data\""))
                {
                    context.Response.Body = originalBodyStream;
                    await context.Response.WriteAsync(body);
                    return;
                }

                object responseBody;

                if (statusCode >= 400)
                {
                    // Handle error responses
                    Dictionary<string, object> errors = null;
                    try
                    {
                        errors = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
                        errors = errors?.ToDictionary(
                        key => key.Key.ToLowerInvariant(), // Convert keys to lowercase
                        value => value.Value
                        );
                    }
                    catch
                    {
                        // If deserialization fails, set a generic error
                        errors = new Dictionary<string, object> { { "Error", body } };
                    }

                    responseBody = new ApiResponse<object>(
                        message: GetErrorMessage(errors),
                        status: statusCode,
                        data: null,
                        errors: errors
                    );
                }
                else
                {
                    // Handle success responses
                    var responseData = JsonSerializer.Deserialize<object>(body);
                    responseBody = new ApiResponse<object>(
                        message: "Success",
                        status: statusCode,
                        data: responseData,
                        errors: null
                    );
                }

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
            if (errors == null || !errors.Any()) return "An error occurred";

            var firstError = errors.FirstOrDefault();
            if (firstError.Key != null && firstError.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
            {
                var messages = JsonSerializer.Deserialize<List<string>>(jsonElement.GetRawText());
                return messages?.FirstOrDefault() ?? "An error occurred";
            }

            return firstError.Key != null ? $"{firstError.Key} is invalid" : "An error occurred";
        }

    }
}
