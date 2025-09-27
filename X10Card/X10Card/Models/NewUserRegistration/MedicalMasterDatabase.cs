using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace X10Card.Models.NewUserRegistration
{
    public class MedicalMasterDatabase
    {
        private SQLiteConnection conn;
        public MedicalMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<MedicalMaster>();
        }

        public IEnumerable<MedicalMaster> GetMedicalMaster(string Querryhere)
        {
            var list = conn.Query<MedicalMaster>(Querryhere);
            return list.ToList();
        }
        public string AddMedicalMaster(MedicalMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteMedicalMaster()
        {
            var del = conn.Query<MedicalMaster>("delete from MedicalMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<MedicalMaster>(query);
            return "success";
        }
    }
}
