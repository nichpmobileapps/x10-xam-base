using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class QualificationMasterDatabase
    {
        private SQLiteConnection conn;
        public QualificationMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<QualificationMaster>();
        }

        public IEnumerable<QualificationMaster> GetQualificationMaster(string Querryhere)
        {
            var list = conn.Query<QualificationMaster>(Querryhere);
            return list.ToList();
        }
        public string AddQualificationMaster(QualificationMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteQualificationMaster()
        {
            var del = conn.Query<QualificationMaster>("delete from QualificationMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<QualificationMaster>(query);
            return "success";
        }
    }
}
