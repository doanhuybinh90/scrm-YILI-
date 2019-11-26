using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Helper
{
    public static class StackExchangeRedisHelper
    {
        private static ConnectionMultiplexer _redis = null;
        private static string RedisServer = null;
        private static int? RedisPort = null;
        private static string RedisPassword = null;
        private static int RedisDb = -1;
        private static object _locker=new object();
        private static string GetDefaultConnectionString()
        {
            RedisServer = StaticObject.RedisServer;
            RedisPort = StaticObject.RedisPort;
            RedisPassword = StaticObject.RedisPassword;
            RedisDb = Convert.ToInt32(StaticObject.RedisDb);
            return StaticObject.RedisConnectString;
        }
        public static ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;

                        _redis = GetManager();
                        return _redis;
                    }
                }

                return _redis;
            }
        }
        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = GetDefaultConnectionString();
            }

            return ConnectionMultiplexer.Connect(connectionString);
        }

        public static string MergeKey(string key,string nameSpace)
        {
            return key = $"n:{nameSpace},c:{key}";
        }
        public static async Task<bool> Remove(string key, string nameSpace)
        {
            key = MergeKey(key, nameSpace);
            var db = Manager.GetDatabase(RedisDb);

            return await db.KeyDeleteAsync(key);
        }

        public static async Task<string> Get(string key, string nameSpace)
        {
            key = MergeKey(key, nameSpace);
            var db = Manager.GetDatabase(RedisDb);
            return await db.StringGetAsync(key);
        }

        public static async Task<bool> Set(string key, string nameSpace, string value, int expireMinutes = 0)
        {
            key = MergeKey(key, nameSpace);
            var db = Manager.GetDatabase(RedisDb);
            bool tOrF = false;
            if (expireMinutes > 0)
            {
                tOrF=await db.StringSetAsync(key, value, TimeSpan.FromMinutes(expireMinutes));
            }
            else
            {
                tOrF=await db.StringSetAsync(key, value);
            }

            return tOrF;
        }

        public static async Task<bool> HasKey(string key, string nameSpace)
        {
            key = MergeKey(key, nameSpace);
            var db = Manager.GetDatabase(RedisDb);

            return await db.KeyExistsAsync(key);
        }
    }
}