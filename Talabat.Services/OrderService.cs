using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Services;

namespace Talabat.Services
{
    internal class OrderService : IOrderService
    {
        public Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, Address ShippingAddress)
        {
            throw new NotImplementedException();
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
