using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models
{
    public class ApplicantDashboardDatabase
    {
        private SQLiteConnection conn;
        public ApplicantDashboardDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ApplicantDashboard>();
        }

        public IEnumerable<ApplicantDashboard> GetApplicantDashboard(string Querryhere)
        {
            var list = conn.Query<ApplicantDashboard>(Querryhere);
            return list.ToList();
        }
        public string AddApplicantDashboard(ApplicantDashboard service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteApplicantDashboard()
        {
            var del = conn.Query<ApplicantDashboard>("delete from ApplicantDashboard");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<ApplicantDashboard>(query);
            return "success";
        }
    }
}
