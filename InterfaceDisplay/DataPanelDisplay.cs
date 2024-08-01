using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataPanelDisplay : MonoBehaviour
{
    #region Variables
    [SerializeField] private MessageDisplay messageDisplay; // Reference to the message display component

    [Header("Inputs")]
    [SerializeField] private Dropdown weaponSelect; // Dropdown menu for selecting weapons
    [SerializeField] private InputDataPanel inputData; // Input fields for weather data
    #endregion


    #region Initialization
    private void Awake()
    {
        // Set culture to invariant for consistent data handling
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        // Initialize data and subscribe to events
        Init();
        WeaponSave.WeaponListModified += WeaponSave_WeaponListModified;
        GPS.LocationSuccess += GPS_LocationSuccess;
        GPS.LocationError += GPS_LocationError;
    }

    private void Init()
    {
        // Populate weapon list, load selected weapon, and load data
        CreateWeaponList();
        weaponSelect.value = WeaponSave.LoadSelected();
        LoadData();
    }

    private void CreateWeaponList()
    {
        // Populate dropdown menu with available weapons
        List<string> weapons = WeaponSave.GetWeaponList();
        weaponSelect.ClearOptions();
        weaponSelect.AddOptions(weapons);
    }
    #endregion


    #region Event Handling
    private void GPS_LocationError(string error)
    {
        // Show error message if location retrieval fails
        messageDisplay.ShowErrorMessage(Messages.LocationError(error));
    }

    private void GPS_LocationSuccess(Vector2 location)
    {
        // Update weather latitude and longitude fields upon successful location retrieval
        inputData.weatherLatitude.text = location.x.ToString();
        inputData.weatherLongtitude.text = location.y.ToString();
        messageDisplay.ShowInfoMessage(Messages.InfoLocationSucces());
    }

    private void WeaponSave_WeaponListModified(object sender, EventArgs e)
    {
        // Reinitialize data when the weapon list is modified
        Init();
    }
    #endregion


    #region Get Location and Weather
    public void Locate()
    {
        // Retrieve current location and display processing message
        messageDisplay.ShowInfoMessage(Messages.InfoProcessingLocation());
        GPS.GetLocation(30, this);
    }

    public async void GetCurrentWeather()
    {
        // Get current weather data based on provided latitude and longitude
        float latitude = float.Parse(inputData.weatherLatitude.text);
        float longitude = float.Parse(inputData.weatherLongtitude.text);

        try
        {
            // Display processing message and retrieve weather data asynchronously
            messageDisplay.ShowInfoMessage(Messages.InfoProcessingWeatherData());
            WeatherData weatherData = await Weather.GetWeatherDataAsync(latitude, longitude);
            // Update UI with current weather data and display success message
            SetCurrentWeather(weatherData.current);
            messageDisplay.ShowInfoMessage(Messages.InfoWeatherDataSucces());
        }
        catch (Exception ex)
        {
            // Display error message if weather data retrieval fails
            messageDisplay.ShowErrorMessage(Messages.WeatherError(ex.Message));
        }
    }

    private void SetCurrentWeather(CurrentData weather)
    {
        // Update weather UI fields with current weather data
        inputData.temperature.text = weather.temperature_2m.ToString();
        inputData.atmosphericPressure.text = weather.surface_pressure.ToString();
        inputData.airHumidity.text = weather.relative_humidity_2m.ToString();
        inputData.windSpeed.text = weather.wind_speed_10m.ToString();
        inputData.windAzimuth.text = weather.wind_direction_10m.ToString();
    }
    #endregion


    #region Save and Load Data
    public void SaveData()
    {
        // Save input data to file
        try
        {
            DataModel data = inputData.Serialize();
            DataSave.Save(data);
        }
        catch (Exception e)
        {
            // Display error message if data save fails
            messageDisplay.ShowErrorMessage(Messages.FileSaveError(DataSave.dataSaveFileName, e.Message));
        }
    }

    private void LoadData()
    {
        // Load data from file
        try
        {
            DataModel data = DataSave.Load();
            inputData.Deserialize(data);
        }
        catch (FileNotFoundException)
        {
            // Ignore if file not found
        }
        catch (Exception e)
        {
            // Display error message if data load fails
            messageDisplay.ShowErrorMessage(Messages.FileReadError(e.Message));
        }
    }
    #endregion


    #region Calculate
    public void Calculate()
    {
        // Calculate trajectory based on selected weapon and input data
        WeaponModel weapon = WeaponSave.Load(weaponSelect.options[weaponSelect.value].text);
        DataModel data = inputData.Serialize();

        try
        {
            Ballistics.CalculateTrajectory(weapon, data);
        }
        catch (Exception e)
        {
            // Display error message if simulation time exceedes maximum time
            messageDisplay.ShowErrorMessage(e.Message);
        }
    }
    #endregion
}
