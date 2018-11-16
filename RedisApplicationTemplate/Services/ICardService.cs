using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleWebsiteRedis.Services
{
    public interface ICardService
    {
        Task Shuffle();
        Task<IList<string>> Deal(int numCards);
    }
}
