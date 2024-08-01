using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

#region ResponseModel

// Serializable class representing the weather data response
[Serializable]
public class WeatherData
{
    public float latitude;
    public float longitude;
    public float generationtime_ms;
    public int utc_offset_seconds;
    public string timezone;
    public string timezone_abbreviation;
    public int elevation;
    public CurrentUnits current_units;
    public CurrentData current;
}

// Serializable class representing the units of the current weather data
[Serializable]
public class CurrentUnits
{
    public string time;
    public string interval;
    public string temperature_2m;
    public string relative_humidity_2m;
    public string surface_pressure;
    public string wind_speed_10m;
    public string wind_direction_10m;
}

// Serializable class representing the current weather data
[Serializable]
public class CurrentData
{
    public string time;
    public int interval;
    public float temperature_2m;
    public int relative_humidity_2m;
    public float surface_pressure;
    public float wind_speed_10m;
    public int wind_direction_10m;
}
#endregion


#region get weather

public static class Weather
{
    // Asynchronous method to fetch weather data based on latitude and longitude
    public static async Task<WeatherData> GetWeatherDataAsync(float latitude, float longitude)
    {
        // Construct the API URL with the provided latitude and longitude
        string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m,surface_pressure,wind_speed_10m,wind_direction_10m&wind_speed_unit=ms";

        using HttpClient httpClient = new();
        httpClient.Timeout = TimeSpan.FromSeconds(10); // Set a 10-second timeout for the HTTP request

        try
        {
            // Send a GET request to the API
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            // If the response is successful, parse the response body into a WeatherData object
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonUtility.FromJson<WeatherData>(responseBody);
                return weatherData;
            }
            else
            {
                // Log the error and throw an exception with the status code
                throw new HttpRequestException($"Failed to get weather data. Status code: {response.StatusCode}");
            }
        }
        catch (TaskCanceledException ex)
        {
            // Log the timeout error and throw a TimeoutException
            throw new TimeoutException("The request to get weather data timed out.", ex);
        }
        catch (Exception ex)
        {
            // Log any other exceptions that occur and rethrow them
            throw;
        }
    }
}
#endregion
