using AutoMapper;
using talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_aggregate;

namespace talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.Category , O => O.MapFrom(S => S.Category.Name))
                .ForMember(D => D.Brand , O => O.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.PictureUrl , O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto , Talabat.Core.Entities.Order_aggregate.Address>();

        }

    }
}
