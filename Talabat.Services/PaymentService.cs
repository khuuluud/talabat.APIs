using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork )
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket> CreateorUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var Basket = await _basketRepository.getBasketAsync(BasketId);
            if (Basket is null) return null;

            //Amount = Subtotal + DM.Cost

            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }

            if(Basket.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
           
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId)) //Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

             paymentIntent =   await Service.CreateAsync(options);
             Basket.PaymentIntentId = paymentIntent.Id;
             Basket.ClientSecret = paymentIntent.ClientSecret;

            }else //Update
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long) SubTotal * 100 + (long)ShippingPrice * 100
                };


               paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId , Options);
               Basket.PaymentIntentId = paymentIntent.Id;
               Basket.ClientSecret = paymentIntent.ClientSecret;
               

            }
            await _basketRepository.UpdateBasketasync(Basket);
            return Basket;
        }
    }
}
