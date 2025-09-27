using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class EmployedDetailsDatabase
    {
        private SQLiteConnection conn;
        public EmployedDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<EmployedDetails>();
        }

        public IEnumerable<EmployedDetails> GetEmployedDetails(string Querryhere)
        {
            var list = conn.Query<EmployedDetails>(Querryhere);
            return list.ToList();
        }
        public string AddEmployedDetails(EmployedDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteEmployedDetails()
        {
            var del = conn.Query<EmployedDetails>("delete from EmployedDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<EmployedDetails>(query);
            return "success";
        }
    }
}
