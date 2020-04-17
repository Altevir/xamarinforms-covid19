using LiteDB;

namespace XFCovid19.Models
{
    public class Country
    {
        public long updated { get; set; }
        [BsonId]
        public string country { get; set; }
        public Countryinfo countryInfo { get; set; }
        public int cases { get; set; }
        public int todayCases { get; set; }
        public int deaths { get; set; }
        public int todayDeaths { get; set; }
        public int? recovered { get; set; }
        public int active { get; set; }
        public int critical { get; set; }
        public int casesPerOneMillion { get; set; }
        public int deathsPerOneMillion { get; set; }
        public int tests { get; set; }
        public int testsPerOneMillion { get; set; }
        public string countryPtBR { get; set; }
    }

    public class Countryinfo
    {
        public int? _id { get; set; }
        public string iso2 { get; set; }
        public string iso3 { get; set; }
        public double lat { get; set; }
        public double _long { get; set; }
        public string flag { get; set; }
    }
}
