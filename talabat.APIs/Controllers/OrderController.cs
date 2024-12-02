using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using talabat.APIs.Dtos;
using talabat.APIs.Errors;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Services;
using Talabat.Services;



namespace talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);


           var Order = await _orderService.CreateOrderAsync(BuyerEmail , orderDto.BasketId , orderDto.DeliveryMethodId , MappedAddress);

            if (Order is null) return BadRequest(new ApiResponse(400 , "There is an error with the order"));

            return Ok(Order);
        }




    }
}
