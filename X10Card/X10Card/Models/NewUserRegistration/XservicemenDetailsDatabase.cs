using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace X10Card.Models.NewUserRegistration
{
    public class XservicemenDetailsDatabase
    {

        private SQLiteConnection conn;
        public XservicemenDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<XservicemenDetails>();
        }

        public IEnumerable<XservicemenDetails> GetXservicemenDetails(string Querryhere)
        {
            var list = conn.Query<XservicemenDetails>(Querryhere);
            return list.ToList();
        }
        public string AddXservicemenDetails(XservicemenDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteXservicemenDetails()
        {
            var del = conn.Query<XservicemenDetails>("delete from XservicemenDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<XservicemenDetails>(query);
            return "success";
        }
    }
}
