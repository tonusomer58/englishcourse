using SozlukApp.Models;
using System.Linq;

namespace SozlukApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SozlukContext context)
        {
            // Ensure database is created (optional, but good practice if not using migrations for creation)
            // context.Database.EnsureCreated(); 

            var adminUser = context.Users.FirstOrDefault(u => u.Username == "tonusomer");

            if (adminUser != null)
            {
                if (adminUser.Role != "Admin")
                {
                    adminUser.Role = "Admin";
                    context.SaveChanges();
                }
            }
            else
            {
                adminUser = new User
                {
                    Username = "tonusomer",
                    Password = "123123", // Default password
                    Role = "Admin"
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
