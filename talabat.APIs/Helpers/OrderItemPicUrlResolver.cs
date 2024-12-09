using AutoMapper;
using System.Security.Cryptography.X509Certificates;
using talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_aggregate;

namespace talabat.APIs.Helpers
{
    public class OrderItemPicUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPicUrlResolver(IConfiguration configuration )
        {
           _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBasUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
