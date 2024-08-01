using System;
using System.Collections;
using UnityEngine;

public static class GPS
{
    #region variables
    // Event triggered on successful location retrieval
    public static event Action<Vector2> LocationSuccess;

    // Event triggered when an error occurs during location retrieval
    public static event Action<string> LocationError;
    #endregion

    #region locate
    // Initiates the location retrieval process
    public static void GetLocation(int maxWaitSec, MonoBehaviour coroutineRunner)
    {
        if (!HasLocationPermission())
        {
            // If location permission is not granted, request it
            RequestLocationPermission(coroutineRunner);
            return;
        }

        // Start the location retrieval coroutine
        coroutineRunner.StartCoroutine(Locate(maxWaitSec));
    }

    // Coroutine to retrieve location data
    private static IEnumerator Locate(int maxWaitSec)
    {
        // Check if location services are enabled by the user
        if (!Input.location.isEnabledByUser)
        {
            LocationError?.Invoke("Location services are not enabled.");
            yield break;
        }

        // Start location service
        Input.location.Start();

        // Wait for the service to initialize
        int maxWait = maxWaitSec;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Check if the service timed out
        if (maxWait < 1)
        {
            LocationError?.Invoke("Timed out while waiting for location data.");
            yield break;
        }

        // Check if the service failed to start
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            LocationError?.Invoke("Failed to determine device location.");
            yield break;
        }
        else
        {
            // Successfully retrieved location data
            Vector2 location = new((float)Input.location.lastData.latitude, (float)Input.location.lastData.longitude);
            LocationSuccess?.Invoke(location);
        }

        // Stop the location service to save power
        Input.location.Stop();
    }
    #endregion

    #region permissions
    // Check if location permission is granted
    private static bool HasLocationPermission()
    {
        return UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation);
    }

    // Request location permission from the user
    private static void RequestLocationPermission(MonoBehaviour coroutineRunner)
    {
        coroutineRunner.StartCoroutine(RequestLocationPermissionCoroutine());
    }

    // Coroutine to request location permission
    private static IEnumerator RequestLocationPermissionCoroutine()
    {
        UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        yield return new WaitUntil(() => UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation));
    }
    #endregion
}
