using System.Collections.Generic;
using System.Linq;
using XFCovid19.Models;
using XFCovid19.Services;

namespace XFCovid19.Helpers
{
    public class Helper
    {
        public static void UpdateDataBase(LiteDbService<Country> db, IEnumerable<Country> countries)
        {
            db.DeleteAll();
            foreach (var item in countries)
                db.UpsertItem(item);
        }
    }
}
