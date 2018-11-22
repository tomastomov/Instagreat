namespace Instagreat.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Data;
    using Data.Models;
    using System.Threading.Tasks;
    using Common.Constants;

    public static class ApplicationBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<InstagreatDbContext>();
                context.Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                Task.Run(async () =>
                {
                    var adminName = RolesConstants.ADMINISTRATOR_ROLE;

                    var exists = await roleManager.RoleExistsAsync(adminName);

                    if (!exists)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = adminName
                        });
                    }

                    var adminUser = await userManager.FindByNameAsync(adminName);

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            UserName = "admin",
                            Email = "admin@admin.com"
                        };

                        await userManager.CreateAsync(adminUser, "admin12");
                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }

                })
                .GetAwaiter()
                .GetResult();
            }

        }

    }
}
