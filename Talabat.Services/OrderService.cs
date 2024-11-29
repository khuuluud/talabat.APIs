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

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
      

        public OrderService(IBasketRepository basketRepo , IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        
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
            var Order = new Order(BuyerEmail , ShippingAddress , DeliveryMethod , OrderItems , SubTotal);

            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);

            //7.Save Order To Database[ToDo]


          var Result =   await _unitOfWork.CompleteAsync();

            if (Result <= 0) return null;

            return Order;

        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
