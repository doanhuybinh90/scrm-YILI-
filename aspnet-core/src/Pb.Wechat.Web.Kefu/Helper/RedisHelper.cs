using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu.Helper
{
    public class RedisHelper
    {
        private static RedisHelper redisHelper = null;
        private static RedisClient redisClient = null;
        private static object lockObject = new object();
        private static int cacheOverSpan = 600;
        private RedisHelper(string host, int port, string password = null, long db = 0)
        {
            redisClient = new RedisClient(host, port, password, db);
        }
        /// <summary>
        /// 单例RedisHelper
        /// </summary>
        /// <param name="redisServer"></param>
        /// <returns></returns>
        public static RedisHelper GetInstance(string host, int port, string password = null, long db = 0)
        {
            if (redisClient == null)
            {
                lock (lockObject)
                {
                    if (redisClient == null)
                    {
                        redisHelper = new RedisHelper(host, port, password, db);
                    }
                }
            }
            return redisHelper;
        }
        public static RedisHelper GetInstance(int cacheOverTime,string host, int port, string password = null, long db = 0)
        {
            cacheOverSpan = cacheOverTime;
            return GetInstance(host, port, password, db);
        }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, string nameSpace = "") 
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            if (redisClient.ContainsKey(key))
                return redisClient.Get<T>(key);
            else
                return default(T);
        }

        /// <summary>
        /// 获取缓存值并更新时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="overSpan"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, int overSpan, string nameSpace = "") 
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            if (overSpan != 0)
                ExpireTime(key, overSpan);
            if (redisClient.ContainsKey(key))
                return redisClient.Get<T>(key);
            else
                return default(T);
        }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, Action action, string nameSpace = "") 
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            var oValue = redisClient.ContainsKey(key) ? redisClient.Get<T>(key) : default(T);
            if (oValue == null)
            {
                if (action != null)
                {
                    action();
                    oValue = redisClient.ContainsKey(key)?redisClient.Get<T>(key): default(T);
                }
            }
            return oValue;
        }
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="tValue"></param>
        /// <param name="nameSpace"></param>
        public void SetValue<T>(string key, T tValue, string nameSpace = "") 
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            redisClient.Set<T>(key, tValue);
            ExpireTime(key, cacheOverSpan);
        }
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="tValue"></param>
        /// <param name="overSpan">过期时间</param>
        /// <param name="nameSpace"></param>
        public void SetValue<T>(string key, T tValue, int overSpan, string nameSpace = "")
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            redisClient.Set<T>(key, tValue);
            ExpireTime(key, overSpan);
        }
        /// <summary>
        /// 更新过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="overSpan"></param>
        /// <param name="nameSpace"></param>
        public void ExpireTime(string key, int overSpan, string nameSpace = "")
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            redisClient.Expire(key, overSpan);
        }
        /// <summary>
        /// 移除缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nameSpace"></param>
        public void RemoveValue(string key, string nameSpace = "")
        {
            if (!string.IsNullOrEmpty(nameSpace))
                key = $"n:{nameSpace},c:{key}";
            redisClient.Remove(key);
        }
    }
}
