using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.DataBaseContext
{
    public class AppContextSql : DbContext
    {
        public AppContextSql(DbContextOptions<AppContextSql> options) : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PermissionType>().HasData(
                new PermissionType { Id = 1, Description = "Vacaciones" },
                new PermissionType { Id = 2, Description = "Viaje" },
                new PermissionType { Id = 3, Description = "Otros" }
            );
        }
    }
}
