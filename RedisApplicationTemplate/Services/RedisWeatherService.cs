using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using WeatherSdk;

namespace ExampleWebsiteRedis.Services
{
    public class RedisWeatherService : WeatherService
    {
        private IConnectionMultiplexer Multiplexer { get; }
        private string WeatherKey(string zip) => $"ZIP{zip}";

        public RedisWeatherService(IConnectionMultiplexer multiplexer)
        {
            Multiplexer = multiplexer;
        }

        public override async Task<WeatherReturnCode> GetWeather(string postalCode)
        {
            WeatherReturnCode ret;
            var db = Multiplexer.GetDatabase();

            // If the item exists in the cache, get it and return it
            if (db.KeyExists(WeatherKey(postalCode)))
            {
                // Create the new appropriate return value
                ret = new WeatherReturnCode();
                var weatherData = await db.HashGetAllAsync(WeatherKey(postalCode));
                ret.Wind = new Wind();
                ret.Wind.Speed = double.Parse(weatherData.FirstOrDefault(a => a.Name == "Wind.Speed").Value.ToString());
                ret.Main = new Main();
                ret.Main.Temp = double.Parse(weatherData.FirstOrDefault(a => a.Name == "Main.Temp").Value.ToString());
                ret.Name = weatherData.FirstOrDefault(a => a.Name == "Name").Value.ToString();
                ret.Cod = 200L;
                return ret;
            }

            ret = await base.GetWeather(postalCode);
            if (ret.CallSuccess)
            {
                db.HashSet(WeatherKey(postalCode), new[]
                {
                    new HashEntry("Wind.Speed", ret.Wind.Speed), 
                    new HashEntry("Main.Temp", ret.Main.Temp), 
                    new HashEntry("Name", ret.Name)
                });
            }
            return ret;
        }
    }
}
