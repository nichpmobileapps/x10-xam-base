using SQLite;
using System;
using System.IO;
using X10Card.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOS_SQLite))]
namespace X10Card.iOS
{
    public class IOS_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "X10Card.db";
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder  
            string libraryPath = Path.Combine(dbPath, "..", "Library"); // Library folder  
            var path = Path.Combine(libraryPath, dbName);
            //Console.WriteLine("Database : " + path);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}
