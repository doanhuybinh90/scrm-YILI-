using System;
using System.Collections.Generic;
using System.Linq;
namespace Pb.Hangfire.Tool
{
    public static class DbTool
    {
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            return int.Parse(obj.ToString());
        }

        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ToNullInt(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return null;
            return int.Parse(obj.ToString());
        }

        public static int DbValueToInt(object newDbValue)
        {
            if (((newDbValue != null) && (newDbValue != DBNull.Value)))
            {
                return Convert.ToInt32(newDbValue);
            }
            return 0;
        }
        /// <summary>
        /// Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            return decimal.Parse(obj.ToString());
        }
        /// <summary>
        /// Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ToNullDecimal(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return null;
            return decimal.Parse(obj.ToString());
        }
        public static decimal DbValueToDecimal(object newDbValue)
        {
            decimal num = 0M;
            try
            {
                if (((newDbValue != null) && (newDbValue != DBNull.Value)))
                {
                    return Convert.ToDecimal(newDbValue);
                }
                num = 0M;
            }
            catch
            {
            }
            return num;
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return string.Empty;
            return obj.ToString();
        }

        public static string ToSqlParamString(object obj)
        {
            return ToString(obj).Replace("'", "''");
        }

        /// <summary>
        /// ToDecimalString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDecimalString(object obj)
        {
            if (obj == DBNull.Value)
                return "0";
            return obj.ToString();
        }

        /// <summary>
        /// ToDateTimeString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateTimeString(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return null;
            return obj.ToString();
        }

        /// <summary>
        /// DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj)
        {
            if (obj == DBNull.Value)
                return DateTime.MaxValue;
            return DateTime.Parse(obj.ToString());
        }

        /// <summary>
        /// DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ToNullDateTime(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return null;
            return DateTime.Parse(obj.ToString());
        }

        /// <summary>
        /// 创建一个按时间排序的Guid
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            //CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime now = DateTime.Now;

            DateTime baseDate = new DateTime(1900, 1, 1);

            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);

            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Copy(daysArray, 0, guidArray, 2, 2);
            //毫秒高位
            Array.Copy(msecsArray, 2, guidArray, 0, 2);
            //毫秒低位
            Array.Copy(msecsArray, 0, guidArray, 4, 2);
            return new Guid(guidArray).ToString();
        }
        /// <summary>
        /// 将实体转化成Insert语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tModel"></param>
        /// <param name="tableName"></param>
        /// <param name="exceptColumnNames"></param>
        /// <returns></returns>
        public static string GetInsertSql<T>(this T tModel, string tableName, params string[] exceptColumnNames) where T : class
        {
            string insertSql = string.Empty;
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            Type type = typeof(T);
            foreach (var prop in type.GetProperties())
            {
                if (!exceptColumnNames.Contains(prop.Name))
                {
                    columns.Add(prop.Name);
                    var obj = prop.GetValue(tModel);
                    if (obj != null)
                        values.Add(obj.ToString());
                    else
                        values.Add("");
                }
            }

            if (columns.Count > 0 && columns.Count == values.Count)
            {
                string _column = $"({string.Join(",", columns)})";
                string _value = $"('{string.Join("','", values)}')";
                insertSql = $"INSERT INTO {tableName} {_column} VALUES {_value}";
                return insertSql;
            }
            else
                return null;
        }
    }
}
