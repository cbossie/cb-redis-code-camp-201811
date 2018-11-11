using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Remotion.Linq.Utilities;
using WeatherSdk;

namespace RedisApplicationTemplate.Pages
{
    public class WeatherModel : TimedPageModel
    {
        private IWeatherService weatherSvc;

        [BindProperty]
        public string TownName { get; set; }
        [BindProperty]
        public string Temperature { get; set; }
        [BindProperty]
        public string WindSpeed { get; set; }
        [BindProperty]
        public bool? Success { get; set; } = null;
        [BindProperty]
        public string Message { get; set; } = null;
        
        [Required]
        [BindProperty]
        public string ZipCode { get; set; }

        public WeatherModel(IWeatherService svc)
        {
            weatherSvc = svc;
        }

        public async void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            Success = null;
            if (!ModelState.IsValid)
            {
                return;
            }

            Start();
            var results = await weatherSvc.GetWeather(ZipCode);
            End();

            Success = results.CallSuccess;
            if (results.CallSuccess)
            {
                TownName = results.Name;
                WindSpeed = results.Wind?.Speed.ToString("0#.##");
                Temperature = results.Main?.Temp.ToString();
            }
            else
            {
                Message = $"Data for Zip {ZipCode} not found";
            }
        }


    }
}
