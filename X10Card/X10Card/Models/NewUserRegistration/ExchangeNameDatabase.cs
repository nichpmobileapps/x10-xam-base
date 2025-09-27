using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class ExchangeNameDatabase
    {
        private SQLiteConnection conn;
        public ExchangeNameDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ExchangeNameMaster>();
        }

        public IEnumerable<ExchangeNameMaster> GetExchangeNameMaster(string Querryhere)
        {
            var list = conn.Query<ExchangeNameMaster>(Querryhere);
            return list.ToList();
        }
        public string AddExchangeNameMaster(ExchangeNameMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteExchangeNameMaster()
        {
            var del = conn.Query<ExchangeNameMaster>("delete from ExchangeNameMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<ExchangeNameMaster>(query);
            return "success";
        }
    }
}
