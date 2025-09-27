using X10Card.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Android_SQLite))]
namespace X10Card.Droid
{
    public class Android_SQLite : ISQLite
    {
        public SQLite.SQLiteConnection GetConnection()
        {
            var dbName = "X10Card.db";
            var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(dbPath, dbName);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
            ///data/user/0/nic.hp.employment/files/X10Card.db
        }
    }
}
