using System.ComponentModel.DataAnnotations;

namespace SozlukApp.Models
{
    public enum WordStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Word
    {
        public int Id { get; set; }

        [Required]
        public string Turkish { get; set; } = string.Empty;

        [Required]
        public string English { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty; // Example sentence or description

        public WordStatus Status { get; set; } = WordStatus.Pending;

        public int? CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }
    }
}
