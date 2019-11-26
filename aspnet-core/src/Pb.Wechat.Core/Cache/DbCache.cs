using Abp.Dependency;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Cache
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class DbCacheAttribute : Attribute, IDbCache, ITransientDependency
    {
        public string CacheKey { get; }
        private readonly ICacheManager _cacheManager;
        public DbCacheAttribute(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        } 
    }
}
