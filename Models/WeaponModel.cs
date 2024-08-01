[System.Serializable]
public class WeaponModel
{
    // Model for storing weapon data

    // General weapon properties
    public string weaponName; // Name of the weapon

    // Bullet properties
    public float caliber; // Caliber of the bullet
    public float bulletMass; // Mass of the bullet
    public float ballisticCoefficient; // Ballistic coefficient of the bullet
    public float bulletLength; // Length of the bullet

    // Barrel properties
    public float threadPitch; // Thread pitch of the barrel
    public float barrelScopeHeight; // Height of the scope above the barrel
    public float unitsPerClick; // Units per click for scope adjustment
    public int correctionUnit; // Correction unit for scope adjustment
    public float muzzleVelocity; // Muzzle velocity of the bullet
    public bool rightHandThread; // Indicates if the barrel has a right-hand thread

    // Zeroing properties
    public float zeroingDistance; // Zeroing distance
    public float zeroingAirPressure; // Air pressure at zeroing
    public float zeroingTemperature; // Temperature at zeroing
    public float zeroingHumidity; // Humidity at zeroing
    public float zeroingAimAzimuth; // Azimuth angle at zeroing
    public float zeroingBarrelElevation; // Barrel elevation angle at zeroing
    public float zeroingWindSpeed; // Wind speed at zeroing
    public float zeroingWindAzimuth; // Wind azimuth at zeroing
    public float zeroingLatitude; // Latitude at zeroing

    // Barrel aim angles
    public float barrelAimAngleX; // Barrel aim angle X
    public float barrelAimAngleY; // Barrel aim angle Y
    public float barrelAimAngleZ; // Barrel aim angle Z
}
