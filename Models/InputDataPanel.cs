using UnityEngine;
using UnityEngine.UI;
using static Validation;

public class InputDataPanel : MonoBehaviour
{
    // Panel for inputting data related to ballistics and weather

    [Header("Basic")]
    public InputField distance; // Input field for distance to target
    public InputField barrelElevation; // Input field for barrel elevation angle
    public InputField aimAzimuth; // Input field for aim azimuth angle

    [Header("Advanced")]
    public InputField windSpeed; // Input field for wind speed
    public InputField windAzimuth; // Input field for wind azimuth angle
    public InputField temperature; // Input field for temperature
    public InputField atmosphericPressure; // Input field for atmospheric pressure
    public InputField airHumidity; // Input field for air humidity
    public InputField latitude; // Input field for latitude

    [Header("GPS")]
    public InputField weatherLatitude; // Input field for weather latitude
    public InputField weatherLongtitude; // Input field for weather longitude

    public DataModel Serialize()
    {
        // Serialize input data into DataModel object

        DataModel data = new()
        {
            // Validate and parse input fields, setting default values if empty
            distance = ValidateAndParseFloat(distance.text, 1f, 5000f, 100f),
            windSpeed = ValidateAndParseFloat(windSpeed.text, 0f, 50f, 0f),
            windAzimuth = ValidateAndParseFloat(windAzimuth.text, 0f, 360f, 0f),
            barrelElevation = ValidateAndParseFloat(barrelElevation.text, -90f, 90f, 0f),
            temperature = ValidateAndParseFloat(temperature.text, -80f, 80f, 15f),
            atmosphericPressure = ValidateAndParseFloat(atmosphericPressure.text, 500f, 1200f, 1013f),
            airHumidity = ValidateAndParseFloat(airHumidity.text, 0f, 100f, 0f),
            latitude = ValidateAndParseFloat(latitude.text, -90f, 90f, 0f),
            aimAzimuth = ValidateAndParseFloat(aimAzimuth.text, 0f, 360f, 0f),
            weatherLatitude = ValidateAndParseFloat(weatherLatitude.text, -90f, 90f, 0f),
            weatherLongtitude = ValidateAndParseFloat(weatherLongtitude.text, -180f, 180f, 0f)
        };

        return data;
    }

    public void Deserialize(DataModel data)
    {
        // Deserialize DataModel object and populate input fields

        distance.text = data.distance.ToString();
        windSpeed.text = data.windSpeed.ToString();
        windAzimuth.text = data.windAzimuth.ToString();
        barrelElevation.text = data.barrelElevation.ToString();
        temperature.text = data.temperature.ToString();
        atmosphericPressure.text = data.atmosphericPressure.ToString();
        airHumidity.text = data.airHumidity.ToString();
        latitude.text = data.latitude.ToString();
        aimAzimuth.text = data.aimAzimuth.ToString();
        weatherLatitude.text = data.weatherLatitude.ToString();
        weatherLongtitude.text = data.weatherLongtitude.ToString();
    }

    public void ResetData()
    {
        // Reset input fields to null

        distance.text = null;
        windSpeed.text = null;
        windAzimuth.text = null;
        barrelElevation.text = null;
        temperature.text = null;
        atmosphericPressure.text = null;
        airHumidity.text = null;
        latitude.text = null;
        aimAzimuth.text = null;
        weatherLatitude.text = null;
        weatherLongtitude.text = null;
    }
}
