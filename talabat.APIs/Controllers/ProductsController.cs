using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;

namespace talabat.APIs.Controllers
{
     
    public class ProductsController : APIBaseController
    {
        private readonly iGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(iGenericRepository<Product> productRepo , IMapper mapper)
        {
            _productRepo = productRepo;
           _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductWithBrandAndTypeSpec();
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var MapperProducts =  _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(Products);
            return Ok(MapperProducts);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(MappedProduct); 
        }
    }
}
