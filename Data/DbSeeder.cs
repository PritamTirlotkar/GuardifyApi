using GuardifyApi.Models;
using BCrypt.Net;

namespace GuardifyApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // 1. CHECK IF SUPERADMIN EXISTS
            var superAdminExists = context.Users
                .Any(x => x.Role == "SuperAdmin");

            if (superAdminExists)
                return; // already seeded

            // 2. CREATE SUPERADMIN
            var superAdmin = new AppUser
            {
                Name = "Super Admin",
                Email = "superadmin@guardify.com",
                Mobile = "9999999999",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "SuperAdmin",
                SocietyId = null,
                shift = null
            };

            // 3. SAVE TO DB
            context.Users.Add(superAdmin);
            context.SaveChanges();
        }
    }
}