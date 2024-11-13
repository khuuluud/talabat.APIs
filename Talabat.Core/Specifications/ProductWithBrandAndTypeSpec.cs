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
        public ProductWithBrandAndTypeSpec(ProductSpecParams Params ) 
            :base(p =>
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || p.BrandId == Params.BrandId)
            &&
            (!Params.CategoryId.HasValue || p.CategoryId == Params.CategoryId)
            )
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
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

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }


        public ProductWithBrandAndTypeSpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }
    }
}
