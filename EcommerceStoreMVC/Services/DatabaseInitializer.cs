using Microsoft.AspNetCore.Identity;

namespace EcommerceStoreMVC.Services
{
    public class DatabaseInitializer
    {
        public static async Task SeedData(UserManager<ApplicationUser>? userManager, RoleManager<IdentityRole>? roleManager)
        {

            if (userManager == null || roleManager == null)
            {
                return;
            }

            SeedRoles(roleManager);
            await SeedData(userManager);
        }

        private static async Task SeedData(UserManager<ApplicationUser> userManager)
        {

            if (userManager.GetUsersInRoleAsync("admin").Result.Count == 0)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    CreatedDate = DateTime.Now,
                };

                string initialPassword = "admin@123";
                IdentityResult result = userManager.CreateAsync(user, initialPassword).Result;
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    Console.WriteLine("Admin user created with password: " + initialPassword);

                }
            }

        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "admin"
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("seller").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "seller"
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("client").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "client"
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

        }

        
    }
}
