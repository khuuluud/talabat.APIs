using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpec : BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpec():base()
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }

        public ProductWithBrandAndTypeSpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Brand);
        }
    }
}
