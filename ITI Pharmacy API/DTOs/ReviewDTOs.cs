using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class ReviewDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isVerifiedPurchase")]
        public bool IsVerifiedPurchase { get; set; }
    }

    public class CreateReviewDTO
    {
        [Required]
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }

    public class ReviewStatusDTO
    {
        [JsonPropertyName("canReview")]
        public bool CanReview { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("isVerifiedPurchase")]
        public bool IsVerifiedPurchase { get; set; }
    }
}
