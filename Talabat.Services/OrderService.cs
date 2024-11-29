using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Repository;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly iGenericRepository<Product> _productRepo;
        private readonly iGenericRepository<DeliveryMethod> _deliveryRepo;
        private readonly iGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepo , iGenericRepository<Product> productRepo , iGenericRepository<DeliveryMethod> deliveryRepo, iGenericRepository<Order> OrderRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryRepo = deliveryRepo;
            _orderRepo = OrderRepo;
        }

        public async Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {

            //1.Get Basket From Basket Repo
            var Basket = await _basketRepo.getBasketAsync(BasketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _productRepo.GetByIdAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrder, item.Quantity, product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _deliveryRepo.GetByIdAsync(DeliveryMethodId);

            //5.Create Order
            var Order = new Order(BuyerEmail , ShippingAddress , DeliveryMethod , OrderItems , SubTotal);

            //6.Add Order Locally
            await _orderRepo.AddAsync(Order);

            //7.Save Order To Database[ToDo]












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
