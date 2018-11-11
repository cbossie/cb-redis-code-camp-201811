using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSdk
{
    public class WeatherService : IWeatherService
    {
        private const string KEY = "707d5f0eef0a03cf2a4baf2af5012489";

        private string RequestUrl(string zipCode) => $"https://api.openweathermap.org/data/2.5/weather?APPID={KEY}&units=imperial&zip={zipCode}";
        private static readonly HttpClient client = new HttpClient();

        public virtual async Task<WeatherReturnCode> GetWeather(string postalCode)
        {
            WeatherReturnCode result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resultString = await client.GetStringAsync(RequestUrl(postalCode));

                result = WeatherReturnCode.FromJson(resultString);

                if (!result.CallSuccess)
                {
                    result.Success = false;
                }
                return result;
            }
            catch (Exception e)
            {
                var code = WeatherReturnCode.GetFailedCode(e.Message);
                return code;
            }
        }


        public virtual async Task<double?> GetTemperatureForLocation(string postalCode)
        {
            double? temperature = null;
            try
            {
                var weather = await GetWeather(postalCode);
                if (weather.CallSuccess && weather.Main != null)
                {
                    temperature = weather.Main.Temp;
                }
            }
            catch (Exception e)
            {
                temperature = null;
            }

            return temperature;
        }

        public virtual async Task<string> GetWeatherString(string postalCode)
        {
            string conditions = string.Empty;

            try
            {
                var weather = await GetWeather(postalCode);
                if (weather.CallSuccess && weather.Weather != null)
                {
                    conditions = string.Join(", ", weather.Weather.Select(w => w.Description));

                }
            }
            catch (Exception e)
            {

            }
            return conditions;


        }
    }
}