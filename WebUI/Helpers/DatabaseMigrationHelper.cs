using DAL.Data.Context;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Helpers
{
    public static class DatabaseMigrationHelper
    {

        public static async Task MigrateToLatestVersion(IApplicationBuilder app)
        {
            throw new Exception("Поооошеееел наахуууй");
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                context.Database.Migrate();
                if (!context.Roles.Any(s => s.Name == "Admin"))
                {
                    var adminRole = new Role
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    context
                        .Roles.Add(adminRole);
                    context
                        .Roles.Add(new Role
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        });
                    context.SaveChanges();
                    var admin = new User
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@admin.com",
                        UserName = "admin@admin.com"
                    };
                    admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, "admin");
                    await userManager.CreateAsync(admin, "admin");
                    context.UserRoles.Add(new UserRole
                    {
                        RoleId = adminRole.Id,
                        UserId = admin.Id
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
