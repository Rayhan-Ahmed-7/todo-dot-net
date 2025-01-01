using System.Text.Json;
using Microsoft.AspNetCore.Http;
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
                    responseBody = new ApiResponse<object>(
                        message: "An error occurred",
                        status: statusCode,
                        data: null,
                        errors: JsonSerializer.Deserialize<object>(body)
                    );
                }
                else
                {
                    responseBody = new ApiResponse<object>(
                        message: "Success",
                        status: statusCode,
                        data: JsonSerializer.Deserialize<object>(body),
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
    }
}
