using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeCampCacheLib;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;

namespace ExampleWebsiteRedis.Services
{
    /// <summary>
    /// Class overrides the original Value Service with methods that implement caching
    /// </summary>
    public class RedisValueService : ValueService
    {
        private IDistributedCache Cache { get; }

        private string FiboKey(int n) => $"FIB{n}";

        public RedisValueService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public override string GetNthFibonacci(int n)
        {
            string nthFib = GetOrRetrieveAndCache(FiboKey(n), () => base.GetNthFibonacci(n));
            return nthFib;
        }

        // Utility Method that Allows us to Abstract Multiple Caching Mechanisms
        private string GetOrRetrieveAndCache(string key, Func<string> retrievalFunction)
        {
            var data = Cache.Get(key);
            if (data != null)
            {
                return Encoding.UTF8.GetString(data);
            }

            var result = retrievalFunction?.Invoke();
            Cache.Set(key, Encoding.UTF8.GetBytes(result));
            return result;
        }

    }
}
