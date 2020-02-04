namespace BertScout2020.Models
{
    public class TeamResult
    {
        public int TeamNumber { get; set; }
        public string Name { get; set; }
        public int TotalRP { get; set; }
        public int TotalScore { get; set; }
        public int AverageScore { get; set; }
        public int TotalPowercells { get; set; }
        public int AveragePowercells { get; set; }
        public int TotalBroken { get; set; }
    }
}
