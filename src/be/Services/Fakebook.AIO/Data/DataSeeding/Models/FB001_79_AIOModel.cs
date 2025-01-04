namespace Fakebook.AIO.DataSeeding.Models
{
    public class SeedData
    {
        public List<CaseData> Cases { get; set; } = null!;
    }

    public class CaseData
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string JobName { get; set; } = null!;
    }
}