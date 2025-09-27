using SQLite;

namespace X10Card
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
