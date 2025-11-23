using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class NotificationDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public required string Message { get; set; } = string.Empty;

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("userId")]
        public required string UserId { get; set; } = string.Empty;
    }

    public class CreateNotificationDTO
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public required string Message { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public required string Type { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public required string UserId { get; set; } = string.Empty;
    }
}