using System;
using System.Collections.Generic;
using UnityEngine;

using static Ballistics;

public class PointMassSimulation : Simulation
{
    #region init
    // Constructor initializing with a WeaponModel
    public PointMassSimulation(WeaponModel weaponModel) : base(weaponModel) { }

    // Constructor initializing with both WeaponModel and DataModel
    public PointMassSimulation(WeaponModel weaponModel, DataModel dataModel) : base(weaponModel, dataModel) { }
    #endregion

    #region simulations
    // Method to calculate the change in velocity vector
    private Vector3 DV(Vector3 V, Vector3 W)
    {
        // Calculate drag force and add gravity to get the total acceleration
        return (p * S * Cd(V.magnitude, formFactor) / (2 * m) * (V - W).magnitude * (V - W)) + g;
    }

    // Override method to simulate a single step of the trajectory
    protected override void SimulateStep()
    {
        // Update velocity with the change in velocity vector
        currentVelocity -= dt * DV(currentVelocity, wind);
        // Update position based on the updated velocity
        currentPosition += dt * currentVelocity;
        // Increment the simulation time
        currentTime += dt;
    }
    #endregion

    #region results handling
    // Calculate results for a specific distance
    protected override ResultModel CalculateResults(float distance)
    {
        float sg = SG(weapon.bulletMass, weapon.threadPitch, weapon.caliber, weapon.bulletLength);
        float spinDrift = SpinDrift(sg, currentTime);
        float aerodynamicJump = AerodynamicJump(sg, weapon.caliber, weapon.bulletLength, wind.magnitude, distance);
        float horizontalCoriolis = HorizontalCoriolisDeflection(distance, currentTime, data.latitude, data.aimAzimuth);
        float verticalCoriolisFactor = CoriolisGravityCorrectionFactor(data.aimAzimuth, data.latitude, currentVelocity.magnitude);


        // Apply corrections to the final position
        Vector3 finalPosition = currentPosition;
        finalPosition.z -= spinDrift * (weapon.rightHandThread ? 1 : -1);
        finalPosition.y *= verticalCoriolisFactor;
        finalPosition.y += aerodynamicJump * (weapon.rightHandThread ? 1 : -1);
        finalPosition.z += horizontalCoriolis;

        // Calculate sight corrections
        Vector2 corrections = CalculateCorrections(finalPosition, weapon.correctionUnit, weapon.unitsPerClick);

        ResultModel result = new ResultModel
        {
            distance = (float)Math.Round(distance),
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
