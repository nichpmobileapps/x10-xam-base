using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class GetLangDetailsDatabase
    {
        private SQLiteConnection conn;
        public GetLangDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<GetLangDetails>();
        }

        public IEnumerable<GetLangDetails> GetGetLangDetails(string Querryhere)
        {
            var list = conn.Query<GetLangDetails>(Querryhere);
            return list.ToList();
        }
        public string AddGetLangDetails(GetLangDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteGetLangDetails()
        {
            var del = conn.Query<GetLangDetails>("delete from GetLangDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<GetLangDetails>(query);
            return "success";
        }
    }
}
