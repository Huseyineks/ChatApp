using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ChatApp.DataAccesLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppUserRole,int>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost;initial catalog=ChatAppDb;integrated Security=true; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity =>
            {
                entity.HasMany(i => i.Messages).WithOne(i => i.Author).HasPrincipalKey(i => i.RowGuid).HasForeignKey(i => i.authorGuid).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Message>(entity =>
            {
                entity.Property(i => i.Status).HasConversion<string>();
            });
        }

        public DbSet<Message> Messages { get; set; } 

        public DbSet<OnlineAppUsers> OnlineUsers { get; set; }

    }
}
