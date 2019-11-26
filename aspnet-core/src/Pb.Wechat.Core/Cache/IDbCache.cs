using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Cache
{
    public interface IDbCache
    {
        string CacheKey { get; }
    }
}
