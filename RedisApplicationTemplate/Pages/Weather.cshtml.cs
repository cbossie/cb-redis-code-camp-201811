using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Remotion.Linq.Utilities;
using StackExchange.Redis;
using WeatherSdk;

namespace RedisApplicationTemplate.Pages
{
    public class WeatherModel : TimedPageModel
    {
        //Add Connection Multiplexor for pub / sub
        private IConnectionMultiplexer Multiplexer { get; }
        

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

        public WeatherModel(IWeatherService svc, IConnectionMultiplexer multiplexer)
        {
            Multiplexer = multiplexer;
            weatherSvc = svc;
        }

        private const string channel = "WEATHER";
        private const string WEATHER_DATA_FOUND = "Found";
        private const string WEATHER_DATA_NOT_FOUND = "Not Found";

        /// <summary>
        /// This method demonstrates the publication
        /// </summary>
        private async Task PublishMessage(string zip, string action, string temperature = null)
        {
            var pub = Multiplexer.GetSubscriber();
            var tempString = temperature == null ? null : $"Temperature = {temperature}";

            // Publish the action to the channel
            await pub.PublishAsync(channel, $"Data for zip {zip} {action}. {tempString}");
          
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

            // If we are getting data for a zipcode, then we will notify!

            Start();
            var results = await weatherSvc.GetWeather(ZipCode);
            End();

            Success = results.CallSuccess;
            if (results.CallSuccess)
            {
                TownName = results.Name;
                WindSpeed = results.Wind?.Speed.ToString("0#.##");
                Temperature = results.Main?.Temp.ToString();

                // Notify!!
                await PublishMessage(ZipCode, WEATHER_DATA_FOUND, Temperature);

            }
            else
            {
                Message = $"Data for Zip {ZipCode} not found";
                await PublishMessage(ZipCode, WEATHER_DATA_NOT_FOUND);
            }
        }


    }
}
