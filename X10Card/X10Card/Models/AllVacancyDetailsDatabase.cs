using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models
{
    public class AllVacancyDetailsDatabase
    {
        private SQLiteConnection conn;
        public AllVacancyDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<AllVacancyDetails>();
        }

        public IEnumerable<AllVacancyDetails> GetAllVacancyDetails(string Querryhere)
        {
            var list = conn.Query<AllVacancyDetails>(Querryhere);
            return list.ToList();
        }
        public string AddAllVacancyDetails(AllVacancyDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteAllVacancyDetails()
        {
            var del = conn.Query<AllVacancyDetails>("delete from AllVacancyDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<AllVacancyDetails>(query);
            return "success";
        }
    }
}
