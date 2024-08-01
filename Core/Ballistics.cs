using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ballistics
{
    #region drag

    // G7 drag coefficient table
    private static readonly Dictionary<float, float> G7Drag = new()
    {
        // Drag coefficients for various Mach numbers
        { 0.00f, 0.1198f },
        { 0.05f, 0.1197f },
        { 0.10f, 0.1196f },
        { 0.15f, 0.1194f },
        { 0.20f, 0.1193f },
        { 0.25f, 0.1194f },
        { 0.30f, 0.1194f },
        { 0.35f, 0.1194f },
        { 0.40f, 0.1193f },
        { 0.45f, 0.1193f },
        { 0.50f, 0.1194f },
        { 0.55f, 0.1193f },
        { 0.60f, 0.1194f },
        { 0.65f, 0.1197f },
        { 0.70f, 0.1202f },
        { 0.725f, 0.1207f },
        { 0.75f, 0.1215f },
        { 0.775f, 0.1226f },
        { 0.80f, 0.1242f },
        { 0.825f, 0.1266f },
        { 0.85f, 0.1306f },
        { 0.875f, 0.1368f },
        { 0.90f, 0.1464f },
        { 0.925f, 0.1660f },
        { 0.95f, 0.2054f },
        { 0.975f, 0.2993f },
        { 1.0f, 0.3803f },
        { 1.025f, 0.4015f },
        { 1.05f, 0.4043f },
        { 1.075f, 0.4034f },
        { 1.10f, 0.4014f },
        { 1.125f, 0.3987f },
        { 1.15f, 0.3955f },
        { 1.20f, 0.3884f },
        { 1.25f, 0.3810f },
        { 1.30f, 0.3732f },
        { 1.35f, 0.3657f },
        { 1.40f, 0.3580f },
        { 1.50f, 0.3440f },
        { 1.55f, 0.3376f },
        { 1.60f, 0.3315f },
        { 1.65f, 0.3260f },
        { 1.70f, 0.3209f },
        { 1.75f, 0.3160f },
        { 1.80f, 0.3117f },
        { 1.85f, 0.3078f },
        { 1.90f, 0.3042f },
        { 1.95f, 0.3010f },
        { 2.00f, 0.2980f },
        { 2.05f, 0.2951f },
        { 2.10f, 0.2922f },
        { 2.15f, 0.2892f },
        { 2.20f, 0.2864f },
        { 2.25f, 0.2835f },
        { 2.30f, 0.2807f },
        { 2.35f, 0.2779f },
        { 2.40f, 0.2752f },
        { 2.45f, 0.2725f },
        { 2.50f, 0.2697f },
        { 2.55f, 0.2670f },
        { 2.60f, 0.2643f },
        { 2.65f, 0.2615f },
        { 2.70f, 0.2588f },
        { 2.75f, 0.2561f },
        { 2.80f, 0.2533f },
        { 2.85f, 0.2506f },
        { 2.90f, 0.2479f },
        { 2.95f, 0.2451f },
        { 3.00f, 0.2424f },
        { 3.10f, 0.2368f },
        { 3.20f, 0.2313f },
        { 3.30f, 0.2258f },
        { 3.40f, 0.2205f },
        { 3.50f, 0.2154f },
        { 3.60f, 0.2106f },
        { 3.70f, 0.2060f },
        { 3.80f, 0.2017f },
        { 3.90f, 0.1975f },
        { 4.00f, 0.1935f },
        { 4.20f, 0.1861f },
        { 4.40f, 0.1793f },
        { 4.60f, 0.1730f },
        { 4.80f, 0.1672f },
        { 5.00f, 0.1618f }
    };

    // Formula for calculating air density in kg/m^3
    public static float CalculateAirDensity(float pressure, float temperature, float humidity)
    {
        float T = temperature + 273.15f; // Convert temperature to Kelvin
        float p1 = 6.1078f * Mathf.Pow(10, (7.5f * temperature) / (temperature + 237.3f));
        float pv = p1 * (humidity / 100f); // Partial pressure of water vapor
        float pd = pressure - pv; // Partial pressure of dry air

        float Rd = 287.058f; // Specific gas constant for dry air
        float Rv = 461.495f; // Specific gas constant for water vapor

        // Calculate air density
        float density = 100 * ((pd / (Rd * T)) + (pv / (Rv * T)));
        return density;
    }

    // Calculate bullet surface area in m^2
    public static float CalculateBulletFrontSurface(float caliber)
    {
        float radius = caliber / 2f; // Radius of the bullet
        float frontSurface = Mathf.PI * Mathf.Pow(radius, 2); // Surface area of the bullet
        return frontSurface;
    }

    // Calculate form factor - dimensionless
    public static float CalculateFormFactor(float Bc, float m, float d)
    {
        float massInGrains = m * 15.432f; // Convert mass to grains
        float diameterInInches = d * 0.03937f; // Convert diameter to inches

        // Calculate form factor
        float formFactor = (massInGrains / 7000.0f) / (Bc * (diameterInInches * diameterInInches));
        return formFactor;
    }

    // Calculate drag coefficient related to velocity and G7 form factor
    public static float Cd(float v, float factor)
    {
        float mach = v / 335f; // Convert velocity to Mach number

        var keys = G7Drag.Keys.OrderBy(x => x).ToList(); // Sorted list of Mach numbers

        // Find the lower and upper keys for interpolation
        float lowerKey = keys.LastOrDefault(x => x <= mach);
        float upperKey = keys.FirstOrDefault(x => x > mach);

        float lowerValue = G7Drag[lowerKey]; // Drag coefficient for the lower key
        float upperValue = G7Drag[upperKey]; // Drag coefficient for the upper key

        // Interpolate drag coefficient
        return Vector2.Lerp(new Vector2(lowerKey, lowerValue), new Vector2(upperKey, upperValue), (mach - lowerKey) / (upperKey - lowerKey)).y * factor;
    }
    
    #endregion


    #region external forces

    // Calculate wind vector based on aim azimuth, wind azimuth, and wind speed
    public static Vector3 CalculateWindVector(float aimAzimuth, float windAzimuth, float windSpeed)
    {
        windAzimuth += 180f;
        windAzimuth %= 360f; // Normalize wind azimuth

        float azimuthDifference = Mathf.DeltaAngle(windAzimuth, aimAzimuth); // Calculate azimuth difference

        // Calculate wind vector components
        float x = Mathf.Cos(Mathf.Deg2Rad * azimuthDifference);
        float y = 0f;
        float z = Mathf.Sin(Mathf.Deg2Rad * azimuthDifference);

        Vector3 windVector = new Vector3(x, y, z) * windSpeed; // Scale by wind speed

        return windVector;
    }

    // Calculate gravity vector based on elevation angle
    public static Vector3 CalculateGravityVector(float elevationAngle)
    {
        float angleRad = Mathf.Deg2Rad * elevationAngle; // Convert angle to radians

        // Calculate gravity vector components
        float gravityX = Mathf.Sin(angleRad) * Physics.gravity.magnitude;
        float gravityY = -Mathf.Cos(angleRad) * Physics.gravity.magnitude;

        return new Vector3(gravityX, gravityY, 0f);
    }
    
    #endregion


    #region simplified equations

    // Calculate stability factor due to Miller Twist Rule
    public static float SG(float m, float threadPitch, float caliber, float length)
    {
        float d = caliber * 0.03937f; // Convert caliber to inches
        float t = threadPitch / d; // Thread pitch
        float l = length / caliber; // Length-to-caliber ratio  

        // Calculate stability factor
        return (462.970752f * m) / (Mathf.Pow(t, 2f) * Mathf.Pow(d, 3f) * l * (1f + Mathf.Pow(l, 2f)));
    }

    // Calculate approximated spin drift in meters
    public static float SpinDrift(float Sg, float time)
    {
        return (Sg + 1.2f) * Mathf.Pow(time, 1.83f) / 31.49606f;
    }

    // Calculate approximated aerodynamic jump in meters
    public static float AerodynamicJump(float Sg, float caliber, float length, float windSpeed, float distance)
    {
        float L = length / caliber;
        float adjustmentMrads = windSpeed * (Sg - 0.24f * L + 3.2f) / 153.681142f;
        return adjustmentMrads * distance / 1000f;
    }

    // Calculate approximated horizontal Coriolis deflection in meters
    public static float HorizontalCoriolisDeflection(float distance, float time, float latitude, float azimuth)
    {
        return distance * Mathf.Sin(Mathf.Deg2Rad * latitude) * Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * azimuth)) * time * 7.292E-5f;
    }

    // Calculate approximated vertical Coriolis deflection as factor of gravity deflection
    public static float CoriolisGravityCorrectionFactor(float azimuth, float latitude, float velocity)
    {
        return 1 - 2 * 7.292E-5f * velocity * Mathf.Cos(Mathf.Deg2Rad * latitude) * Mathf.Sin(Mathf.Deg2Rad * azimuth) / Mathf.Abs(Physics.gravity.y);
    }
    #endregion


    #region results

    // Calculate corrections for aim based on final position
    public static Vector2 CalculateCorrections(Vector3 finalPosition, int correctionUnit, float unitsPerClick)
    {
        float verticalCorrection = finalPosition.y;
        float horizontalCorrection = finalPosition.z;

        // Select multiplier based on correction unit
        (float multiplier, _) = correctionUnit switch
        {
            0 => (1000f, Mathf.Rad2Deg), // Milliradians
            1 => (Mathf.Rad2Deg * 60f, Mathf.Rad2Deg), // MOA
            _ => throw new ArgumentException("Unknown angular unit: " + correctionUnit)
        };

        // Calculate corrections in chosen units
        verticalCorrection = Mathf.Atan2(verticalCorrection, finalPosition.x) * multiplier / unitsPerClick;
        horizontalCorrection = Mathf.Atan2(horizontalCorrection, finalPosition.x) * multiplier / unitsPerClick;

        // Round corrections to nearest unit
        verticalCorrection = Mathf.Round(verticalCorrection);
        horizontalCorrection = Mathf.Round(horizontalCorrection);

        return new Vector2(horizontalCorrection, verticalCorrection);
    }

    // Calculate kinetic energy
    public static float E(float m, float v)
    {
        return (float)(m * Math.Pow(v, 2) / 2);
    }

    // Event triggered when calculations are finished
    public static event Action<List<ResultModel>> OnCalculatingFinished;
    #endregion


    #region calculations

    // Calculate barrel aim angle for weapon
    public static Vector3 CalculateBarrelAimAngle(WeaponModel weapon)
    {
        int maxSteps = 5;

        // Initialize barrel aim angles
        weapon.barrelAimAngleX = 1f;
        weapon.barrelAimAngleY = 0f;
        weapon.barrelAimAngleZ = 0f;

        Vector3 hitPoint;

        while (maxSteps > 0)
        {
            Simulation sim = new PointMassSimulation(weapon);
            hitPoint = sim.SimplifiedTrajectory().normalized; // Normalize the hit point

            // Adjust aim based on hit point error
            Vector3 newAim = new(1, weapon.barrelAimAngleY - hitPoint.y, weapon.barrelAimAngleZ - hitPoint.z);
            newAim.Normalize();

            // Update barrel aim angles
            weapon.barrelAimAngleX = newAim.x;
            weapon.barrelAimAngleY = newAim.y;
            weapon.barrelAimAngleZ = newAim.z;

            maxSteps--;
        }

        Vector3 result = new(weapon.barrelAimAngleX, weapon.barrelAimAngleY, weapon.barrelAimAngleZ);

        return result.normalized; // Return normalized result
    }

    // Calculate trajectory for weapon
    public static void CalculateTrajectory(WeaponModel weapon, DataModel data)
    {
        Simulation sim = new PointMassSimulation(weapon, data);
        List<ResultModel> result = sim.ComplexTrajectory(); // Compute the complex trajectory
        OnCalculatingFinished.Invoke(result); // Invoke the event with the result
    }
    #endregion
}
