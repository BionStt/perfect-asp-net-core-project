using System;
using System.Collections.Generic;
using System.Text;
using DAL.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebUI;

namespace UnitTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                 options.UseInMemoryDatabase("TestDatabase"));
        }
    }
}
