using System.Text.Json.Serialization;

namespace TodoApp.Application.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("errors")]
        public object Errors { get; set; }

        public ApiResponse(string message, int status, T data, object errors)
        {
            Message = message;
            Status = status;
            Data = data;
            Errors = errors;
        }
    }
}
