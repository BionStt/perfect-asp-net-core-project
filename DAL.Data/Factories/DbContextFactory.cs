using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL.Data.Factories
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<Context.ApplicationContext>
    {
        public Context.ApplicationContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<Context.ApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new Context.ApplicationContext(optionsBuilder.Options);

        }
    }
}
