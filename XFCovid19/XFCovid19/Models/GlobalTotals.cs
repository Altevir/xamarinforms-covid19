using LiteDB;

namespace XFCovid19.Models
{
    public class GlobalTotals
    {
        [BsonId]
        public string globalKey { get; set; }
        public long updated { get; set; }
        public int cases { get; set; }
        public int todayCases { get; set; }
        public int deaths { get; set; }
        public int todayDeaths { get; set; }
        public int recovered { get; set; }
        public int active { get; set; }
        public int critical { get; set; }
        public int casesPerOneMillion { get; set; }
        public float deathsPerOneMillion { get; set; }
        public int tests { get; set; }
        public float testsPerOneMillion { get; set; }
        public int affectedCountries { get; set; }
    }

}
