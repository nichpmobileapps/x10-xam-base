using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace X10Card.Models.NewUserRegistration
{
   public class ContactDetailsDatabase
    {
        private SQLiteConnection conn;
        public ContactDetailsDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ContactDetails>();
        }

        public IEnumerable<ContactDetails> GetContactDetails(string Querryhere)
        {
            var list = conn.Query<ContactDetails>(Querryhere);
            return list.ToList();
        }
        public string AddContactDetails(ContactDetails service)
        {
            conn.Insert(service);
            return "success";
        }
        public string DeleteContactDetails()
        {
            var del = conn.Query<ContactDetails>("delete from ContactDetails");
            return "success";
        }
        public string UpdateCustomquery(string query)
        {
            var update = conn.Query<ContactDetails>(query);
            return "success";
        }
    }
}
