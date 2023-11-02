using Microsoft.AspNetCore.Identity;
using PersonalBlog.Models;

namespace PersonalBlog.Data
{
    public static class RoleInitializer
    {
        public static async Task InitializerAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                if (await roleManager.FindByNameAsync("Admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (await roleManager.FindByNameAsync("Editor") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Editor"));
                }
            }
            if (!userManager.Users.Any())
            {
                var users = new[]
                {
                    new { Email = "admin@gmail.com",Name = "Admin", LastName= "Adminovich",Password = "qwerty"},
                    new { Email = "alex@gmail.com",Name = "Tom",LastName="Cruiz",Password = "asdf123"},
                    new { Email = "marry@gmail.com",Name = "Marry",LastName="Smith",Password = "11111"},
                };

                foreach (var user in users)
                {
                    if (await userManager.FindByEmailAsync(user.Email)==null)
                    {
                        User currentUser = new User
                        {
                            Email = user.Email,
                            UserName = user.Email,
                            FirstName = user.Name, 
                            LastName = user.LastName,
                        };
                        IdentityResult identityResult = await userManager.CreateAsync(currentUser,user.Password);
                        if (identityResult.Succeeded)
                        {
                            if (currentUser.Email.Equals("admin@gmail.com"))
                            {
                                await userManager.AddToRoleAsync(currentUser, "Admin");
                            }
                            else
                            {
                                await userManager.AddToRoleAsync(currentUser, "Editor");
                            }
                            
                        }
                    }
                }
            }
        }
    }
}
