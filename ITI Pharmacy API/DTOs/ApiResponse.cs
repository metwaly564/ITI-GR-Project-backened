using System;
using System.Collections.Generic;

namespace ITI.Pharmacy.API.DTOs
{

 
    public class AdvancedApiResponse<T>
    {
        
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

     
        public T? Data { get; set; }

      
        public List<string>? Errors { get; set; }

       
        public PaginationResponse<T>? Pagination { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }


 
   
    public class PaginationResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<T> Items { get; set; } = new List<T>();
    }
}
