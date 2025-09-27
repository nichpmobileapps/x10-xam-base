using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models
{
    public class SubEmploymentStatusDatabase
    {
        private SQLiteConnection conn;
        public SubEmploymentStatusDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<SubEmploymentStatus>();
        }

        public IEnumerable<SubEmploymentStatus> GetSubEmploymentStatus(string Querryhere)
        {
            var list = conn.Query<SubEmploymentStatus>(Querryhere);
            return list.ToList();
        }
        public string AddSubEmploymentStatus(SubEmploymentStatus service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteSubEmploymentStatus()
        {
            var del = conn.Query<SubEmploymentStatus>("delete from SubEmploymentStatus");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<SubEmploymentStatus>(query);
            return "success";
        }
    }
}
