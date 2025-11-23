using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs.Responses
{
    public class PaginationResponse<T>
    {
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        [JsonPropertyName("hasPrevious")]
        public bool HasPrevious => CurrentPage > 1;

        [JsonPropertyName("hasNext")]
        public bool HasNext => CurrentPage < TotalPages;

        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();

        // Constructors
        public PaginationResponse() { }

        public PaginationResponse(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        // Static factory method
        public static PaginationResponse<T> Create(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            return new PaginationResponse<T>(items, totalCount, currentPage, pageSize);
        }

        // Method to create empty pagination
        public static PaginationResponse<T> Empty(int pageSize = 10)
        {
            return new PaginationResponse<T>
            {
                Items = new List<T>(),
                TotalCount = 0,
                CurrentPage = 1,
                PageSize = pageSize
            };
        }
    }
}