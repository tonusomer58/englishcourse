namespace SozlukApp.Models
{
    public class AdminPanelViewModel
    {
        public List<Word> Words { get; set; } = new List<Word>();
        public List<TestResult> TestResults { get; set; } = new List<TestResult>();
        
        // Statistics
        public double AverageScore { get; set; }
        public int TotalTestsTaken { get; set; }
        public int TotalUsersTested { get; set; }
    }
}
