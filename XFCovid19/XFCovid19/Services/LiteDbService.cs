using LiteDB;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace XFCovid19.Services
{
    public class LiteDbService<T>
    {
        protected LiteCollection<T> _collection;
        LiteDatabase _db;

        public LiteDbService()
        {
            _db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "covid19.db"));
            _collection = _db.GetCollection<T>();
            _collection.EnsureIndex("country");
            _collection.EnsureIndex("countryPtBR");
        }

        public IEnumerable<T> FindAll()
        {
            return _collection.FindAll();
        }

        public bool UpsertItem(T item)
        {
            return _collection.Upsert(item);
        }

        public bool DeleteAll()
        {
            return _collection.Delete(Query.All()) > 0;
        }
    }
}
