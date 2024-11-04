using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;

namespace talabat.APIs.Controllers
{
     
    public class ProductsController : APIBaseController
    {
        private readonly iGenericRepository<Product> _productRepo;

        public ProductsController(iGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductWithBrandAndTypeSpec();
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            //OkObjectResult result = new OkObjectResult(Products);
            //return result;
            return Ok(Products);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);
            return Ok(product);
        }
    }
}
