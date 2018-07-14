using System;
using System.Collections.Generic;
using System.Text;
using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebUI;

namespace UnitTests
{
    public class TestStartup : Startup
    {
        public TestStartup(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                 options.UseInMemoryDatabase("TestDatabase"));
        }
    }
}
