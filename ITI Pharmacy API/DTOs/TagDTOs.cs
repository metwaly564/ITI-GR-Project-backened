using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    /// <summary>
    /// DTO for Tags - used for product filtering
    /// Note: Tags are currently extracted from products, but this DTO allows for future tag entity implementation
    /// </summary>
    public class TagDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nameEn")]
        public string NameEn { get; set; } = string.Empty;

        [JsonPropertyName("nameAr")]
        public string NameAr { get; set; } = string.Empty;
    }
}

