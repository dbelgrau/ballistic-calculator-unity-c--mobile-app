using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static Ballistics;

public abstract class Simulation
{
    #region variables
    // Simulation time step and maximum simulation time
    protected float dt = 0.001f;
    protected float maxTime = 10f;

    // Weapon and data models
    protected WeaponModel weapon;
    protected DataModel data;

    // Ballistic properties
    protected Vector3 aimBarrelAngle;
    protected Vector3 wind;
    protected Vector3 g; // Gravity vector
    protected float p; // Air density
    protected float S; // Bullet front surface area
    protected float m; // Bullet mass
    protected float formFactor; // Drag form factor

    // Current state of the simulation
    protected Vector3 currentPosition;
    protected Vector3 currentVelocity;
    protected float currentTime;
    #endregion

    #region init
    // Constructor initializing with a WeaponModel
    public Simulation(WeaponModel weaponModel)
    {
        InitValues(weaponModel);
    }

    // Constructor initializing with both WeaponModel and DataModel
    public Simulation(WeaponModel weaponModel, DataModel dataModel)
    {
        InitValues(weaponModel, dataModel);
    }

    // Calculate ballistic properties
    private void CalculateBallisticProperties()
    {
        m = weapon.bulletMass / 1000f;
        S = CalculateBulletFrontSurface(weapon.caliber / 1000f);
        p = CalculateAirDensity(data.atmosphericPressure, data.temperature, data.airHumidity);
        wind = CalculateWindVector(data.aimAzimuth, data.windAzimuth, data.windSpeed);
        g = CalculateGravityVector(data.barrelElevation);
        formFactor = CalculateFormFactor(weapon.ballisticCoefficient, weapon.bulletMass, weapon.caliber);
    }

    // Initialize values using WeaponModel and DataModel
    private void InitValues(WeaponModel weaponModel, DataModel dataModel)
    {
        weapon = weaponModel;
        data = dataModel;
        CalculateBallisticProperties();

        // Initialize simulation state
        currentPosition = new Vector3(0, weapon.barrelScopeHeight / 1000f, 0);
        Vector3 barrelAimAngle = new Vector3(weapon.barrelAimAngleX, weapon.barrelAimAngleY, weapon.barrelAimAngleZ);
        currentVelocity = barrelAimAngle * weapon.muzzleVelocity;
        currentTime = 0f;
    }

    // Initialize values using WeaponModel with default DataModel values
    private void InitValues(WeaponModel weaponModel)
    {
        DataModel newData = new DataModel
        {
            distance = weaponModel.zeroingDistance,
            airHumidity = weaponModel.zeroingHumidity,
            temperature = weaponModel.zeroingTemperature,
            aimAzimuth = weaponModel.zeroingAimAzimuth,
            atmosphericPressure = weaponModel.zeroingAirPressure,
            windSpeed = weaponModel.zeroingWindSpeed,
            windAzimuth = weaponModel.zeroingWindAzimuth,
            barrelElevation = weaponModel.zeroingBarrelElevation,
            latitude = weaponModel.zeroingLatitude
        };

        InitValues(weaponModel, newData);
    }
    #endregion

    #region trajectories methods
    // Complex trajectory calculation with detailed results
    public List<ResultModel> ComplexTrajectory()
    {
        List<ResultModel> resultList = new List<ResultModel>();
        float[] distances = new float[] { 0.25f, 0.5f, 0.75f, 0.95f, 1.0f, 1.05f };
        bool[] distanceFlags = new bool[distances.Length];

        while (currentPosition.x < data.distance * distances.Last() && currentTime < maxTime)
        {
            if (currentTime > maxTime)
            {
                throw new InvalidOperationException("Simulation time exceeded the maximum allowed time.");
            }

            SimulateStep();

            for (int i = 0; i < distances.Length; i++)
            {
                float stepDistance = data.distance * distances[i];
                if (!distanceFlags[i] && currentPosition.x >= stepDistance)
                {
                    ResultModel result = CalculateResults(stepDistance);
                    resultList.Add(result);
                    distanceFlags[i] = true;
                }
            }
        }

        return resultList;
    }


    // Simplified trajectory calculation returning the final position
    public Vector3 SimplifiedTrajectory()
    {
        while (currentPosition.x < data.distance && currentTime < maxTime)
        {
            if (currentTime > maxTime)
            {
                throw new InvalidOperationException("Simulation time exceeded the maximum allowed time.");
            }
            
            SimulateStep();
        }

        return new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
    }
    #endregion

    #region define calculation method
    // Abstract method to be implemented by subclasses to simulate a single step
    protected abstract void SimulateStep();
    #endregion

    #region results handling
    // Calculate results for a specific distance
    protected virtual ResultModel CalculateResults(float distance)
    {
        // Calculate sight corrections
        Vector2 corrections = CalculateCorrections(currentPosition, weapon.correctionUnit, weapon.unitsPerClick);

        ResultModel result = new ResultModel
        {
            distance = (float)Math.Round(distance * data.distance),
            x = corrections.x,
            y = corrections.y,
            energy = (int)Math.Round(E(m, currentVelocity.magnitude)),
            velocity = (int)Math.Round(currentVelocity.magnitude),
            time = currentTime
        };
        return result;
    }
    #endregion
}