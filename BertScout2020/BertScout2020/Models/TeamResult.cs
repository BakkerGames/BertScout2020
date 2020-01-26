namespace BertScout2020.Models
{
    public class TeamResult
    {
        public int TeamNumber { get; set; }
        public string Name { get; set; }
        public int TotalRP { get; set; }
        public int AverageScore { get; set; }
        public int TotalPowercells { get; set; }
        public int AveragePowercells { get; internal set; }
    }
}
