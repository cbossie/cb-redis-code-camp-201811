using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ExampleWebsiteRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Remotion.Linq.Utilities;
using WeatherSdk;

namespace RedisApplicationTemplate.Pages
{
    public class CardsModel : TimedPageModel
    {
        private ICardService CardSvc { get; }
        
        public int? Count { get; set; }

        [BindProperty]
        public bool? Success { get; set; } = null;

        [BindProperty]
        public string Message { get; set; } = null;

        [BindProperty]
        public List<string> Cards { get; } = new List<string>();

        public CardsModel(ICardService svc)
        {
            CardSvc = svc;

        }

        public async Task OnGet()
        {

        }

        public async Task OnPostShuffle()
        {
            Start();
            await CardSvc.Shuffle();
            End();
            Message = "Deck Shuffled!!";
            Success = null;
        }

        public async Task OnPostDeal(int? count)
        {
            Count = count.GetValueOrDefault();
            Cards.Clear();
            Start();
            var dealt = await CardSvc.Deal(count.GetValueOrDefault());
            End();
            if (dealt.Any())
            {
                Success = true;
                Cards.AddRange(dealt);                    
            }
            else
            {
                Success = false;
                Message = "No Cards left in the deck";
            }

        }




    }
}
