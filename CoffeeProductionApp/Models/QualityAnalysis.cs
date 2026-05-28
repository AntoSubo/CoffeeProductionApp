namespace CoffeeProductionApp.Models
{
    public class QualityAnalysis
    {
        public int Id { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string SampleType { get; set; }
        public int? GreenBeanBatchId { get; set; }
        public int? FinishedProductBatchId { get; set; }
        public string Parameters { get; set; }
        public decimal? CuppingScore { get; set; }
        public string Conclusion { get; set; }

        public string GreenBatchNumber { get; set; }
        public string FinishedBatchNumber { get; set; }
    }
}