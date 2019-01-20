using ExampleWebsiteRedis.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleWebsiteRedis.Services
{
    public class LocationService : ILocationService
    {
        private IConnectionMultiplexer  Multiplexer { get; }

        public LocationService(IConnectionMultiplexer mux)
        {
            Multiplexer = mux;
        }

        public async Task<IEnumerable<MemberDistance>> GetCitiesWithinNMiles(string name, double miles)
        {
            List<MemberDistance> cities = new List<MemberDistance>();

            var db = Multiplexer.GetDatabase();
            try
            {
                var data = await db.GeoRadiusAsync("Maine", name, miles, GeoUnit.Miles);
                foreach (var d in data)
                {
                    MemberDistance dist = new MemberDistance()
                    {
                        MemberName = d.Member,
                        Distance = d.Distance
                    };
                    cities.Add(dist);
                }
            }
            catch(Exception ex)
            {
                //TODO Log
            }
            
            return cities;
        }
    }
}
