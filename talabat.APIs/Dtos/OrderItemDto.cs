using Talabat.Core.Entities.Order_aggregate;

namespace talabat.APIs.Dtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string productName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}