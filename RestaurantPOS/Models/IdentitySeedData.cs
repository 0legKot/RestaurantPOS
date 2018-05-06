using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{

    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async Task EnsurePopulated(UserManager<IdentityUser> userManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin");
                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}