using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// MSSqlHelper 的摘要说明
/// </summary>
public class MSSqlHelper:IDisposable
{
    public MSSqlHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    private static MSSqlHelper myhelper = null;
    private static SqlConnection mycon = null;
    private static object sqlLock = new object();
    private static string _connectionString = null;
    public MSSqlHelper(string connectionString)
    {
        _connectionString = connectionString;
        //mycon = new SqlConnection(_connectionString);
    }

    /// <summary>
    /// 返回数据集
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public DataSet QueryDataSet(string sqlString)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlDataAdapter adap = new SqlDataAdapter(sqlString, conn);
            conn.Open();
            DataSet ds = new DataSet();

            adap.Fill(ds);
            return ds;
        }
    }
    /// <summary>
    /// 返回表
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public DataTable QueryDataTable(string sqlString)
    {
        return QueryDataSet(sqlString).Tables[0];
    }
    /// <summary>
    /// 执行语句
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public int ExecString(string sqlString)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            int resultInt = -1;

            SqlCommand sqlComm = new SqlCommand(sqlString, conn);
            SqlTransaction tran = conn.BeginTransaction();
            conn.Open();
            try
            {
                resultInt = sqlComm.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return resultInt;
        }
    }
    /// <summary>
    /// 查询返回对象列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public List<T> QueryEntityList<T>(string sqlString) where T : class
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            List<T> tCollection = new List<T>();

            SqlCommand sqlComm = new SqlCommand(sqlString, conn);
            conn.Open();
            SqlDataReader reader = sqlComm.ExecuteReader();
            while (reader.Read())
            {

                T tValue = Activator.CreateInstance<T>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    prop.SetValue(tValue, reader[prop.Name]);
                }
                tCollection.Add(tValue);

            }
            reader.Close();
            conn.Close();
            return tCollection;
        }
    }
    /// <summary>
    /// 查询返回对象列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public List<T> QueryEntityList<T>(string tableName, params string[] parameters) where T : class
    {
        string columns = string.Join(",", typeof(T).GetProperties().Select(m => m.Name).ToArray());
        string paraNames = string.Join(" AND ", parameters);
        string sqlString = string.Format("SELECT {0} FROM {1} {2}", columns, tableName, !string.IsNullOrEmpty(paraNames) ? string.Format(" WHERE {0}", paraNames) : null);
        return QueryEntityList<T>(sqlString);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString">目标连接字符</param>
    /// <param name="TableName">目标表</param>
    /// <param name="dt">源数据</param>
    public void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                try
                {
                    sqlbulkcopy.DestinationTableName = TableName;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    sqlbulkcopy.WriteToServer(dt);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
    ~MSSqlHelper()
    {
        Dispose();
    }

    public void Dispose()
    {
        mycon = null;
    }
}