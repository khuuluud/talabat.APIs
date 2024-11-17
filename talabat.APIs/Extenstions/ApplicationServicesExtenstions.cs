using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Errors;
using talabat.APIs.Helpers;
using Talabat.Core.Repository;
using Talabat.Repository;

namespace talabat.APIs.Extenstions
{
    public static class ApplicationServicesExtenstions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof( IBasketRepository), typeof( BasketRepository));

            Services.AddScoped(typeof(iGenericRepository<>), typeof(GenericRepository<>));

            Services.AddAutoMapper(typeof(MappingProfiles));

           Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {


                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();

                    var ValidationErrorResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return Services;
        }
    }
}
