using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class DataModel
{
    // Data model for storing input data

    // Ballistics data
    public float distance; // Distance to target
    public float windSpeed; // Wind speed
    public float windAzimuth; // Wind direction

    // Gun data
    public float barrelElevation; // Barrel elevation angle
    public float temperature; // Ambient temperature
    public float atmosphericPressure; // Atmospheric pressure
    public float airHumidity; // Air humidity

    // Location data
    public float latitude; // Latitude of firing position
    public float aimAzimuth; // Azimuth angle of the aim

    // Weather data
    public float weatherLatitude; // Latitude for weather data retrieval
    public float weatherLongtitude; // Longitude for weather data retrieval
}
