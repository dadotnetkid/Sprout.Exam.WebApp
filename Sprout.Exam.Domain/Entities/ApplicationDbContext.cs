using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.Domain.Models;

namespace Sprout.Exam.Domain.Entities
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public DbSet<Employee> Employees { get; set; }
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>(e =>
            {
                e.ToTable("Employee");
                e.HasKey(x => x.Id);
                e.HasOne(x => x.EmployeeType).WithMany().HasForeignKey(x => x.EmployeeTypeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            base.OnModelCreating(builder);
        }
    }
}
