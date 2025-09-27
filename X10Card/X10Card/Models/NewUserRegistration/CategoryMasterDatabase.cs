using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace X10Card.Models.NewUserRegistration
{
    public class CategoryMasterDatabase
    {
        private SQLiteConnection conn;
        public CategoryMasterDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<CategoryMaster>();
        }

        public IEnumerable<CategoryMaster> GetCategoryMaster(string Querryhere)
        {
            var list = conn.Query<CategoryMaster>(Querryhere);
            return list.ToList();
        }
        public string AddCategoryMaster(CategoryMaster service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteCategoryMaster()
        {
            var del = conn.Query<CategoryMaster>("delete from CategoryMaster");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<CategoryMaster>(query);
            return "success";
        }
    }
}
