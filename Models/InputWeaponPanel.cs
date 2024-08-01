using UnityEngine;
using UnityEngine.UI;
using static Validation;

public class InputWeaponPanel : MonoBehaviour
{
    // Panel for inputting weapon data

    [Header("Name")]
    public InputField weaponName; // Input field for weapon name

    [Header("Ammunition")]
    public InputField caliber; // Input field for bullet caliber
    public InputField bulletMass; // Input field for bullet mass
    public InputField ballisticCoefficient; // Input field for ballistic coefficient
    public InputField bulletLength; // Input field for bullet length

    [Header("Weapon")]
    public InputField threadPitch; // Input field for barrel thread pitch
    public InputField barrelScopeHeight; // Input field for scope height above the barrel
    public InputField unitsPerClick; // Input field for units per click for scope adjustment
    public Dropdown correctionUnit; // Dropdown for correction unit selection
    public InputField muzzleVelocity; // Input field for muzzle velocity
    public Dropdown threadDirection; // Dropdown for thread direction selection

    [Header("Zeroing")]
    public InputField zeroingDistance; // Input field for zeroing distance
    public InputField zeroingAirPressure; // Input field for air pressure at zeroing
    public InputField zeroingTemperature; // Input field for temperature at zeroing
    public InputField zeroingHumidity; // Input field for humidity at zeroing
    public InputField zeroingLatitude; // Input field for latitude at zeroing
    public InputField zeroingAimAzimute; // Input field for aim azimuth at zeroing
    public InputField zeroingBarrelElevation; // Input field for barrel elevation at zeroing
    public InputField zeroingWindSpeed; // Input field for wind speed at zeroing
    public InputField zeroingWindAzimuth; // Input field for wind azimuth at zeroing

    public WeaponModel Serialize()
    {
        // Serialize input data into WeaponModel object

        WeaponModel weapon = new WeaponModel
        {
            weaponName = weaponName.text,
            caliber = ValidateAndParseFloat(caliber.text, 1f, 20f, 9.0f),
            bulletMass = ValidateAndParseFloat(bulletMass.text, 0.1f, 200f, 11f),
            ballisticCoefficient = ValidateAndParseFloat(ballisticCoefficient.text, 0.01f, 1f, 0.2f),
            bulletLength = ValidateAndParseFloat(bulletLength.text, 0.1f, 100f, 30f),

            threadPitch = ValidateAndParseFloat(threadPitch.text, 1f,20f, 11f),
            barrelScopeHeight = ValidateAndParseFloat(barrelScopeHeight.text, 1f, 200f, 4f),
            unitsPerClick = ValidateAndParseFloat(unitsPerClick.text, 0.01f, 10f, 0.25f),
            correctionUnit = correctionUnit.value,
            muzzleVelocity = ValidateAndParseFloat(muzzleVelocity.text, 1f, 2000f, 800f),
            rightHandThread = threadDirection.value == 0,

            zeroingDistance = ValidateAndParseFloat(zeroingDistance.text, 1f, 3000f, 100f),
            zeroingAirPressure = ValidateAndParseFloat(zeroingAirPressure.text, 500f, 1200f, 1013f),
            zeroingTemperature = ValidateAndParseFloat(zeroingTemperature.text, -80f, 80f, 15f),
            zeroingHumidity = ValidateAndParseFloat(zeroingHumidity.text, 0f, 100f, 0.2f),
            zeroingLatitude = ValidateAndParseFloat(zeroingLatitude.text, -90f, 90f, 0f),
            zeroingAimAzimuth = ValidateAndParseFloat(zeroingAimAzimute.text, 0f, 360f, 0f),
            zeroingBarrelElevation = ValidateAndParseFloat(zeroingBarrelElevation.text, -90f, 90f, 0f),
            zeroingWindSpeed = ValidateAndParseFloat(zeroingWindSpeed.text, 0f, 50f, 0f),
            zeroingWindAzimuth = ValidateAndParseFloat(zeroingWindAzimuth.text, 0f, 360f, 0f)
        };

        return weapon;
    }

    public void Deserialize(WeaponModel weapon)
    {
        // Deserialize WeaponModel object and populate input fields

        weaponName.text = weapon.weaponName;
        caliber.text = weapon.caliber.ToString();
        bulletMass.text = weapon.bulletMass.ToString();
        ballisticCoefficient.text = weapon.ballisticCoefficient.ToString();
        bulletLength.text = weapon.bulletLength.ToString();

        threadPitch.text = weapon.threadPitch.ToString();
        barrelScopeHeight.text = weapon.barrelScopeHeight.ToString();
        unitsPerClick.text = weapon.unitsPerClick.ToString();
        correctionUnit.value = weapon.correctionUnit;
        muzzleVelocity.text = weapon.muzzleVelocity.ToString();
        threadDirection.value = weapon.rightHandThread ? 0 : 1;

        zeroingDistance.text = weapon.zeroingDistance.ToString();
        zeroingAirPressure.text = weapon.zeroingAirPressure.ToString();
        zeroingTemperature.text = weapon.zeroingTemperature.ToString();
        zeroingHumidity.text = weapon.zeroingHumidity.ToString();
        zeroingLatitude.text = weapon.zeroingLatitude.ToString();
        zeroingAimAzimute.text = weapon.zeroingAimAzimuth.ToString();
        zeroingBarrelElevation.text = weapon.zeroingBarrelElevation.ToString();
        zeroingWindSpeed.text = weapon.zeroingWindSpeed.ToString();
        zeroingWindAzimuth.text = weapon.zeroingWindAzimuth.ToString();
    }

    public void ResetInput()
    {
        // Reset input fields to null

        weaponName.text = null;
        caliber.text = null;
        bulletMass.text = null;
        ballisticCoefficient.text = null;
        bulletLength.text = null;

        threadPitch.text = null;
        barrelScopeHeight.text = null;
        unitsPerClick.text = null;
        correctionUnit.value = 0;
        muzzleVelocity.text = null;
        threadDirection.value = 0;

        zeroingDistance.text = null;
        zeroingAirPressure.text = null;
        zeroingTemperature.text = null;
        zeroingHumidity.text = null;
        zeroingLatitude.text = null;
        zeroingAimAzimute.text = null;
        zeroingBarrelElevation.text = null;
        zeroingWindSpeed.text = null;
        zeroingWindAzimuth.text = null;
    }
}
