using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace X10Card.Models.NewUserRegistration
{
    public class TehsilMasterDatabase
    {

        private SQLiteConnection conn;
        public TehsilMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<TehsilMaster>();
        }

        public IEnumerable<TehsilMaster> GetTehsilMaster(string Querryhere)
        {
            var list = conn.Query<TehsilMaster>(Querryhere);
            return list.ToList();
        }
        public string AddTehsilMaster(TehsilMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteTehsilMaster()
        {
            var del = conn.Query<TehsilMaster>("delete from TehsilMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<TehsilMaster>(query);
            return "success";
        }
    }
}
