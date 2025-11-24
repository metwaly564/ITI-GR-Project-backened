using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        public required string ShippingAddress { get; set; } = string.Empty;

        public required string CustomerNotes { get; set; } = string.Empty;


        public IFormFile? PrescriptionImage { get; set; }

        [Required]
        public required string PaymentMethod { get; set; } = "Cash";

        public int BranchId { get; set; }


        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }

}
