using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_aggregate
{
    public class ProductItemOrder
    {
        public int ProductId { get; set; }
        public string productName { get; set; }
        public string PictureUrl { get; set; }
    }
}
