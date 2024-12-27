using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Dtos;
using talabat.APIs.Errors;
using Talabat.Core.Services;

namespace talabat.APIs.Controllers
{

    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }



        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> createOrUpdatePaymentIntent(string basketId)
        {
            var Basket = await _paymentService.CreateorUpdatePaymentIntent(basketId);

            if (Basket is null) return BadRequest(new ApiResponse(400 , "There is an error with your basket"));

            return Ok(Basket); 
        }

    }
}
