using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace talabat.APIs.Extenstions
{
    public static class UserMangerExtenstion
    {
        public static async Task<AppUser> FindUserAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await  userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == Email);
            return user;
        }
    }
}
