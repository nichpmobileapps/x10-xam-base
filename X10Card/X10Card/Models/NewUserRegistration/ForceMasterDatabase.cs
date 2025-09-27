using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class ForceMasterDatabase
    {
        private SQLiteConnection conn;
        public ForceMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ForceMaster>();
        }

        public IEnumerable<ForceMaster> GetForceMaster(string Querryhere)
        {
            var list = conn.Query<ForceMaster>(Querryhere);
            return list.ToList();
        }
        public string AddForceMaster(ForceMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteForceMaster()
        {
            var del = conn.Query<ForceMaster>("delete from ForceMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<ForceMaster>(query);
            return "success";
        }
    }
}
