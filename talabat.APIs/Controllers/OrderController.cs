﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using talabat.APIs.Dtos;
using talabat.APIs.Errors;
using Talabat.Core;
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
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderService orderService, IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);


            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

            if (Order is null) return BadRequest(new ApiResponse(400, "There is an error with the order"));

            return Ok(Order);
        }


        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetUserOrder()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderForSpecificUserAsync(BuyerEmail);
            if (orders is null) return NotFound(new ApiResponse(400, "There is no orders for this user"));

            var MappedOrders =  _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(MappedOrders);

        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
      
        public async Task<ActionResult<Order>> GetuserOrderById(int id)
        {

            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order =  await _orderService.GetOrderForSpecificUserAsync(BuyerEmail , id);

            if (order is null) return NotFound(new ApiResponse(400, $"There is no order with {id} for this user"));
            var MappedOrders =  _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(MappedOrders); 

        }

        [HttpGet("DeliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        }
    }
}
