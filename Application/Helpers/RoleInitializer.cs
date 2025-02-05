using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Helpers
{
    public static class RoleInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (RolesEnum roleEnumValue in Enum.GetValues(typeof(RolesEnum)))
            {
                string roleName = roleEnumValue.ToString();
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}