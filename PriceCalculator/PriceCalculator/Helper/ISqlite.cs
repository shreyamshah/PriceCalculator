
using SQLite;

namespace PriceCalculator.Helpers
{
    public interface ISqlite
    {
        SQLiteAsyncConnection GetConnection();
    }
}