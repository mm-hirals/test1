using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataEntities
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(x => x.UserId).UseIdentityColumn();
                b.Property(x => x.UserId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Lookups> Lookups { get; set; }
        public DbSet<Contractors> Contractors { get; set; }
        public DbSet<SubjectTypes> SubjectTypes { get; set; }
        public DbSet<LookupValues> LookupValues { get; set; }
        public DbSet<ContractorCategoryMapping> ContractorCategoryMapping { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }
        public DbSet<AccessoriesType> AccessoriesTypes { get; set; }
        public DbSet<Accessories> Accessories { get; set; }
        public DbSet<RawMaterial> RawMaterial { get; set; }
        public DbSet<Fabrics> Fabric { get; set; }
        public DbSet<Woods> Wood { get; set; }
        public DbSet<Polish> Polish { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<UserTenantMapping> UserTenantMapping { get; set; }
    }
}