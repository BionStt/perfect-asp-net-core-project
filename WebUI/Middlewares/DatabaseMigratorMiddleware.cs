using DAL.Data.Context;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middlewares
{
    public class DatabaseMigratorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private static bool _initialized = false;

        public DatabaseMigratorMiddleware(RequestDelegate next,
                                      ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext httpcontext, ApplicationContext _context, UserManager<User> _userManager)
        {
            if (_initialized)
            {
                await _next(httpcontext);
                return;
            }
            _context.Database.Migrate();
            if (!_context.Roles.Any(s => s.Name == "Admin"))
            {
                var adminRole = new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                _context.Roles.Add(adminRole);
                _context.Roles.Add(new Role
                {
                    Name = "User",
                    NormalizedName = "USER"
                });
                _context.SaveChanges();
                var admin = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };
                admin.PasswordHash = _userManager.PasswordHasher.HashPassword(admin, "admin");
                await _userManager.CreateAsync(admin, "admin");
                _context.UserRoles.Add(new UserRole
                {
                    RoleId = adminRole.Id,
                    UserId = admin.Id
                });
                _context.SaveChanges();
            }
            _initialized = true;
            await _next(httpcontext);
        }
    }
}
