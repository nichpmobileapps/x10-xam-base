using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models
{
    public class SponsorshipDetailsDatabase
    {
        private SQLiteConnection conn;
        public SponsorshipDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<SponsorshipDetails>();
        }

        public IEnumerable<SponsorshipDetails> GetSponsorshipDetails(string Querryhere)
        {
            var list = conn.Query<SponsorshipDetails>(Querryhere);
            return list.ToList();
        }
        public string AddSponsorshipDetails(SponsorshipDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteSponsorshipDetails()
        {
            var del = conn.Query<SponsorshipDetails>("delete from SponsorshipDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<SponsorshipDetails>(query);
            return "success";
        }
    }
}
