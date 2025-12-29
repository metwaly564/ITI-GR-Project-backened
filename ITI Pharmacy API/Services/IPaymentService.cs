using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd");
    }
}
