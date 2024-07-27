namespace RadicalExam.Models
{
    public class BanxicoSerie
    {
        public string IdSerie { get; set; }
        public string Title { get; set; }
        public List<BanxicoRecord> Records { get; set; } = new();
    }
}
