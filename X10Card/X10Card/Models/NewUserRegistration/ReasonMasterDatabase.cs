using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class ReasonMasterDatabase
    {
        private SQLiteConnection conn;
        public ReasonMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ReasonMaster>();
        }

        public IEnumerable<ReasonMaster> GetReasonMaster(string Querryhere)
        {
            var list = conn.Query<ReasonMaster>(Querryhere);
            return list.ToList();
        }
        public string AddReasonMaster(ReasonMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteReasonMaster()
        {
            var del = conn.Query<ReasonMaster>("delete from ReasonMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<ReasonMaster>(query);
            return "success";
        }
    }
}
