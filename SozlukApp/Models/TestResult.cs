using System;
using System.ComponentModel.DataAnnotations;

namespace SozlukApp.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public string Level { get; set; } = string.Empty;
        public DateTime DateTaken { get; set; }
        
        public User? User { get; set; }
    }
}
