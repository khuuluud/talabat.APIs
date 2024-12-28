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
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo , IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {

            //1.Get Basket From Basket Repo
            var Basket = await _basketRepo.getBasketAsync(BasketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrder, item.Quantity, product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //5.Create Order
            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
               await _paymentService.CreateorUpdatePaymentIntent(BasketId);
                
            }
;
            var Order = new Order(BuyerEmail , ShippingAddress , DeliveryMethod , OrderItems , SubTotal , Basket.PaymentIntentId);

            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);

            //7.Save Order To Database[ToDo]


            var Result =   await _unitOfWork.CompleteAsync();

            if (Result <= 0) return null;

            return Order;

        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);

            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }

        public async Task<Order?> GetOrderForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(BuyerEmail , OrderId);

            var orders = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return orders;
        }
    }
}
