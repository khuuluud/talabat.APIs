using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Dtos;
using talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;


namespace talabat.APIs.Controllers
{
     
    public class ProductsController : APIBaseController
    {
        private readonly iGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly iGenericRepository<ProductCategory> _typeRepo;
        private readonly iGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(iGenericRepository<Product> productRepo , IMapper mapper , iGenericRepository<ProductCategory> typeRepo , iGenericRepository<ProductBrand> brandRepo)
        {
            _productRepo = productRepo;
           _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var spec = new ProductWithBrandAndTypeSpec();
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var MapperProducts =  _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            return Ok(MapperProducts);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(MappedProduct); 
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetTypes()
        {
            var types = await _typeRepo.GetAllAsync();
                return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {

            var brands = await _brandRepo.GetAllAsync();
            return Ok(brands);

        }
    }
}
