﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpec : BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpec(string Sort , int? BrandId , int? CategoryId ) 
            :base(p =>
            (!BrandId.HasValue || p.BrandId == BrandId)
            &&
            (!CategoryId.HasValue || p.CategoryId == CategoryId)
            )
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
        }


        public ProductWithBrandAndTypeSpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }
    }
}
