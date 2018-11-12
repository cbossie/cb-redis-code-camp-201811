using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core;
using WeatherSdk;

namespace ExampleWebsiteRedis.Services
{
    public class RedisExtensionsWeatherService : WeatherService
    {
        private ICacheClient CacheClient { get; }

        public RedisExtensionsWeatherService(ICacheClient client)
        {
            CacheClient = client;
        }

        public string WeatherKey(string zip) => $"ZipExt{zip}";

        public override async Task<WeatherReturnCode> GetWeather(string postalCode)
        {
            // Check to see if the value is already in the cache
            var val = await CacheClient.GetAsync<WeatherReturnCode>(WeatherKey(postalCode));
            if (val != null)
            {
                return val;
            }

            // Not found, get the "real" value
            var ret = await base.GetWeather(postalCode);
            if (ret.CallSuccess)
            {
                await CacheClient.AddAsync(WeatherKey(postalCode), ret);
            }

            return ret;
        }
    }
}
