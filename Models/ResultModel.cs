using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultModel
{
    // Model for storing ballistic calculation results

    // Trajectory data
    public float distance; // Distance traveled
    public float x; // Horizontal correction (clicks)
    public float y; // Vertical correction (clicks)

    // Projectile properties
    public int energy; // Energy of the projectile
    public int velocity; // Velocity of the projectile

    // Time data
    public float time; // Time of flight
}
