using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductCountAsync : BaseSpecifications<Product>
    {
        public ProductCountAsync(ProductSpecParams Params)
            :base(p =>
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || p.BrandId == Params.BrandId)
            &&
            (!Params.CategoryId.HasValue || p.CategoryId == Params.CategoryId)
            )
        {
        }
    }
}
