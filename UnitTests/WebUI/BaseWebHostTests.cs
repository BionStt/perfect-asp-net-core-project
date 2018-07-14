using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UnitTests.WebUI
{
    public class BaseWebHostTests
    {
        protected readonly TestServer Server;
        protected readonly IServiceScope Scope;
        protected readonly IServiceProvider Services;

        public BaseWebHostTests()
        {
            var builder =
                WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseEnvironment("Development");
            Server = new TestServer(builder);
            Scope = Server.Host.Services.CreateScope();
            Services = Scope.ServiceProvider;
        }
    }
}
