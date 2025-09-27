using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class PHDetailsDatabase
    {
        private SQLiteConnection conn;
        public PHDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<PHDetails>();
        }

        public IEnumerable<PHDetails> GetPHDetails(string Querryhere)
        {
            var list = conn.Query<PHDetails>(Querryhere);
            return list.ToList();
        }
        public string AddPHDetails(PHDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeletePHDetails()
        {
            var del = conn.Query<PHDetails>("delete from PHDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<PHDetails>(query);
            return "success";
        }
    }
}
