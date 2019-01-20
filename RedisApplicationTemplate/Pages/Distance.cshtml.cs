using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleWebsiteRedis.Model;
using ExampleWebsiteRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RedisApplicationTemplate;

namespace ExampleWebsiteRedis.Pages
{
    public class DistanceModel : TimedPageModel
    {
        [BindProperty]
        public double Distance { get; set; }
        [BindProperty]
        public string TownName { get; set; }

        [BindProperty]
        public bool? Success { get; set; } = null;

        [BindProperty]
        public string Message { get; set; } = null;

        [BindProperty]
        public List<MemberDistance> Distances { get; } = new List<MemberDistance>();

        ILocationService Svc { get; }

        public DistanceModel(ILocationService svc)
        {
            Svc = svc;
        }

        public async Task OnGet()
        {

        }

        public async Task OnPostDistance(string town, double dist)
        {

            TownName = town;
            Distance = dist;
            Distances.Clear();
            Start();
            var data = await Svc.GetCitiesWithinNMiles(town, dist);
            End();

            if (data.Any())
            {
                Success = true;
                Distances.AddRange(data);
            }
            else
            {
                Success = false;
                Message = "Nothing found in that distance";
            }



        }

    }
}