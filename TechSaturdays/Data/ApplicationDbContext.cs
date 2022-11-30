using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using TechSaturdays.Models;

namespace TechSaturdays.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        private ILogger<ApplicationDbContext> _logger;
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Application> Applications { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(a => new { a.GroupId, a.UserId });
                entity.HasOne(a => a.User).WithMany(u => u.Applications).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.Creator).WithMany(u => u.CreatedApplications).HasForeignKey(a => a.CreatorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Revoker).WithMany(u => u.RevokedApplications).HasForeignKey(a => a.RevokerId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
