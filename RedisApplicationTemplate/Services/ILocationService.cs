using ExampleWebsiteRedis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleWebsiteRedis.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<MemberDistance>> GetCitiesWithinNMiles(string name, double miles);
    }
}
