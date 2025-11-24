using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.Models.Responses
{
    public class AdvancedApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        public AdvancedApiResponse() { }

        public AdvancedApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
            Timestamp = DateTime.UtcNow;
        }

        public static AdvancedApiResponse<T> CreateSuccess(T data, string message = "Operation completed successfully")
        {
            return new AdvancedApiResponse<T>(true, message, data);
        }

        public static AdvancedApiResponse<T> CreateError(string message, List<string>? errors = null)
        {
            return new AdvancedApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                Timestamp = DateTime.UtcNow
            };
        }

        public static AdvancedApiResponse<T> CreateNotFound(string resourceName = "Resource")
        {
            return new AdvancedApiResponse<T>
            {
                Success = false,
                Message = $"{resourceName} not found",
                Timestamp = DateTime.UtcNow
            };
        }
    }


    public class AdvancedApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static AdvancedApiResponse CreateSuccess(string message = "Operation completed successfully")
        {
            return new AdvancedApiResponse
            {
                Success = true,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }

        public static AdvancedApiResponse CreateError(string message, List<string>? errors = null)
        {
            return new AdvancedApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
