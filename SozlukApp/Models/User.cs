using System.ComponentModel.DataAnnotations;

namespace SozlukApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty; // In a real app, this should be hashed.
        
        public string Role { get; set; } = "User"; // "Admin" or "User"
    }
}
