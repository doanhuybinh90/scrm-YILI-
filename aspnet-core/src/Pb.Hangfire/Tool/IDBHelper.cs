using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Hangfire.Tool
{
    public interface IDBHelper
    {
        int ExcuteNonQuery(string cmd, DynamicParameters param = null, bool flag = true);
        Task<int> ExcuteNonQueryAsync(string cmd, DynamicParameters param = null, bool flag = true);
        object ExecuteScalar(string cmd, DynamicParameters param = null, bool flag = true);
        Task<object> ExecuteScalarAsync(string cmd, DynamicParameters param = null, bool flag = true);
        T FindOne<T>(string cmd, DynamicParameters param = null, bool flag = true) where T : class, new();
        Task<T> FindOneAsync<T>(string cmd, DynamicParameters param = null, bool flag = true) where T : class, new();
        IList<T> FindToList<T>(string cmd, DynamicParameters param = null, bool flag = true) where T : class, new();
        Task<IList<T>> FindToListAsync<T>(string cmd, DynamicParameters param = null, bool flag = true) where T : class, new();
        IList<T> FindToListAsPage<T>(string cmd, DynamicParameters param = null, bool flag = true) where T : class, new();
    }
}
