using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace talabat.APIs.Extenstions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection Services)
        {

            Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication();
            return Services;
        }
    }
}
