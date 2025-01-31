using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestBackEnd.Infrastructure.Configuration;

namespace TestBackEnd.Infrastructure
{
    public class TestDBContext : IdentityDbContext<IdentityUser>
    {

        public TestDBContext(DbContextOptions<TestDBContext> options): base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}