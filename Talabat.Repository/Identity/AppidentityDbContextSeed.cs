using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppidentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Khlud Sayed",
                    Email = "khuuluudsayed18@gmail.com",
                    UserName = "khuuluudsayed18",
                    PhoneNumber = "123456789",

                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            
        }
    }
}
