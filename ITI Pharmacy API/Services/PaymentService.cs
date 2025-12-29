using Stripe;
using Microsoft.Extensions.Configuration;

namespace ITI.Pharmacy.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd")
        {
            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe expects amount in cents
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var intent = await service.CreateAsync(options);
            return intent.ClientSecret;
        }
    }
}
