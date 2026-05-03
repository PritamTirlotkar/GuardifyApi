using Microsoft.EntityFrameworkCore;
using GuardifyApi.Models;
using System.Net.Sockets;

namespace GuardifyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 🧑 Users table
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Society> Societies { get; set; }
        public DbSet<Residence> Residences { get; set; }   // ✅ ADD THIS

    }
}