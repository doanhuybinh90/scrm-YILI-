using Abp.Runtime.Caching;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Cache
{
    public class DbCachingInterceptor : IInterceptor
    {
        private readonly ICacheManager _cacheManager;

        public DbCachingInterceptor(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsAsyncMethod(invocation.Method))
            {
                InterceptAsync(invocation);
            }
            else
            {
                InterceptSync(invocation);
            }
        }



        private void InterceptAsync(IInvocation invocation)
        {
            //Before method execution
            var stopwatch = Stopwatch.StartNew();

            //Calling the actual method, but execution has not been finished yet
            invocation.Proceed();

            //We should wait for finishing of the method execution
            ((Task)invocation.ReturnValue)
                .ContinueWith(task =>
                {
                    //After method execution
                    stopwatch.Stop();
                });
        }

        private void InterceptSync(IInvocation invocation)
        {
            //Before method execution
            var stopwatch = Stopwatch.StartNew();

            //Executing the actual method
            invocation.Proceed();

            //After method execution
            stopwatch.Stop();
        }

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        private void ProceedCaching(IInvocation invocation, QCachingAttribute attribute)
        {
            var cacheValue = _cacheManager.Get(cacheKey);
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cacheManager.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
            }
        }
    }
}
