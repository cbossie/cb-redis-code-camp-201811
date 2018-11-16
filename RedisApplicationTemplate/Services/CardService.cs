using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ExampleWebsiteRedis.Services
{
    public class CardService : ICardService
    {
        private IConnectionMultiplexer Multiplexer { get; }
        public CardService(IConnectionMultiplexer multiplexer)
        {
            Multiplexer = multiplexer;
        }

        private string Key = "DECK_OF_CARDS";

        public async Task Shuffle()
        {
            var DB = Multiplexer.GetDatabase();
            var lines = System.IO.File.ReadAllLines("Cards.txt");
            foreach (var line in lines)
            {
                await DB.SetAddAsync(Key, line);
            }            
        }



        public async Task<IList<string>> Deal(int numCards)
        {
            if (numCards <= 0)
            {
                return new List<string>();
            }


            List<string> cards = new List<string>();



            var DB = Multiplexer.GetDatabase();
            for (int i = 0; i < numCards; i++)
            {
                var card = await DB.SetPopAsync(Key);
                if (card.HasValue)
                {
                    cards.Add(card.ToString());
                }
                else
                {
                    break;
                }

            }
            return cards;
        }
    }
}
