using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Helpers
{
    public static class AdminInitializer
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@farah.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail.ToLower(),
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    SSN = "11111111111111",
                    GovID = 1,
                    CityID = 1,
                    YourFavirotePerson = "admin",

                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RolesEnum.Admin.ToString());
                }
            }
        }
    }
}
