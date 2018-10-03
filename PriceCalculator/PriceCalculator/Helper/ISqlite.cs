
using SQLite;

namespace PriceCalculator.Helpers
{
    public interface ISqlite
    {
        SQLiteConnection GetConnection();
    }
}