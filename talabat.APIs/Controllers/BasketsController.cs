using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repository;

namespace talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Basket = await _basketRepository.getBasketAsync(BasketId);
            return Basket is null ? new CustomerBasket(BasketId) : Ok(Basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> updateBasket(CustomerBasket Basket)
        {

           var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketasync(Basket);

            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);

        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
          return  await _basketRepository.DeleteBasketAsync(BasketId);

        }


    }
}
