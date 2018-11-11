using System;
using System.Threading.Tasks;

namespace WeatherSdk
{
    public interface IWeatherService
    {
        Task<WeatherReturnCode> GetWeather(string postalCode);
        Task<double?> GetTemperatureForLocation(string postalCode);
        Task<string> GetWeatherString(string postalCode);
    }
}