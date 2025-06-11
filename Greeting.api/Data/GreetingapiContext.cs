using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Greeting.api.Model;

namespace Greeting.api.Data
{
    public class GreetingapiContext : DbContext
    {
        public GreetingapiContext (DbContextOptions<GreetingapiContext> options)
            : base(options)
        {
        }

        public DbSet<Greeting.api.Model.Greetings> Greetings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Greetings>().HasData(new Greetings
            {
                Id = 1,
                Name = "World",
                Greeting = "Hello, World",
                CreatedAt = DateTime.UtcNow
            });
        }

    }
}
