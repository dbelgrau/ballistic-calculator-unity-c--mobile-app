using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class Test
{
    #region  Ballistics - drag

    [Test]
    public void CalculateAirDensity_WhenCalled_ReturnsCorrectDensity()
    {
        float result, expected;
        float epsilon = 1E-4f;

        result = Ballistics.CalculateAirDensity(1013f, 15f, 30f);
        expected = 1.222366f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.CalculateAirDensity(900f, -20f, 100f);
        expected = 1.237866f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.CalculateAirDensity(1100f, 40f, 20f);
        expected = 1.217504f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void CalculateBulletFrontSurface_WhenCalled_ReturnsCorrectSurfaceArea()
    {
        float result, expected;
        float epsilon = 1E-5f;

        result = Ballistics.CalculateBulletFrontSurface(0.00762f);
        expected = 0.0000456f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.CalculateBulletFrontSurface(0.009f);
        expected = 0.00006362f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void CalculateFormFactor_WhenCalled_ReturnsCorrectFormFactor()
    {
        float result, expected;
        float epsilon = 1E-6f;

        result = Ballistics.CalculateFormFactor(0.2f, 11f, 9f);
        expected = 0.965764f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void Cd_WhenCalled_ReturnsCorrectDragCoefficient()
    {
        float result, expected;
        float epsilon = 1E-4f;

        result = Ballistics.Cd(0f, 1f);
        expected = 0.1198f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.Cd(335f, 1f);
        expected = 0.3803f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.Cd(994.94f, 1f);
        expected = 0.24402f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.Cd(994.94f, 1.1f);
        expected = 0.268422f;
        Assert.AreEqual(expected, result, epsilon);
    }
    
    #endregion


    #region Ballistics - external forces
    
    [Test]
    public void CalculateWindVector_WhenCalled_ReturnsCorrectWindVector()
    {
        Vector3 result, expected;
        Vector3EqualityComparer comparer = new(1E-4f);

        result = Ballistics.CalculateWindVector(0f, 90f, 10f);
        expected = new Vector3(0f, 0f, 10f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));

        result = Ballistics.CalculateWindVector(180f, 90f, 5f);
        expected = new Vector3(0f, 0f, -5f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));

        result = Ballistics.CalculateWindVector(189f, 34f, 2.35f);
        expected = new Vector3(2.129823f, 0f, -0.993153f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));
    }

    [Test]
    public void CalculateGravityVector_WhenCalled_ReturnsCorrectGravityVector()
    {
        Vector3 result, expected;
        Vector3EqualityComparer comparer = new(1E-4f);

        result = Ballistics.CalculateGravityVector(0f);
        expected = new Vector3(0f, -9.81f, 0f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));

        result = Ballistics.CalculateGravityVector(50f);
        expected = new Vector3(7.51489f, -6.305746f, 0f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));

        result = Ballistics.CalculateGravityVector(-10f);
        expected = new Vector3(-1.703489f, -9.660964f, 0f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));
    }

    #endregion


    #region Ballistics - simplified equations
    
    // examples from Applied Ballistics for Long Range Shooting by Bryan Litz, converted to civilized units
    [Test]
    public void SG_WhenCalled_ReturnsCorrectStabilityFactor()
    {
        float result, expected;
        float epsilon = 1E-2f;

        result = Ballistics.SG(10.0438f, 13f, 7.8232f, 30.48f);
        expected = 1.42f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void SpinDrift_WhenCalled_ReturnsCorrectSpinDrift()
    {
        float result, expected;
        float epsilon = 1E-2f;

        result = Ballistics.SpinDrift(1.5f, 1.6845f);
        expected = 0.22352f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void AerodynamicJump_WhenCalled_ReturnsCorrectAerodynamicJump()
    {
        float result, expected;
        float epsilon = 1E-2f;

        result = Ballistics.AerodynamicJump(1.49f, 7f, 38.5318f, 3.57632f, 1000f);
        expected = 0.0785398f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void HorizontalCoriolisDeflection_WhenCalled_ReturnsCorrectDeflection()
    {
        float result, expected;
        float epsilon = 1E-5f;

        result = Ballistics.HorizontalCoriolisDeflection(914.4f, 1.5f, 45f, 0f);
        expected = 0.0707227f;
        Assert.AreEqual(expected, result, epsilon);
    }

    [Test]
    public void CoriolisGravityCorrectionFactor_WhenCalled_ReturnsCorrectCorrectionFactor()
    {
        float result, expected;
        float epsilon = 1E-4f;

        result = Ballistics.CoriolisGravityCorrectionFactor(90f, 45f, 914.4f);
        expected = 0.9904f;
        Assert.AreEqual(expected, result, epsilon);
    }
    
    #endregion


    #region Ballistics - results

    [Test]
    public void CalculateCorrections_WhenCalled_ReturnsCorrectCorrections()
    {
        Vector2 result, expected;
        Vector2EqualityComparer comparer = new(1E-3f);

        result = Ballistics.CalculateCorrections(new Vector3(100f, 1f, 0.2f), 0, 1f);
        expected = new Vector2(2f, 10f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));

        result = Ballistics.CalculateCorrections(new Vector3(100f, -0.0290888f, -2.90888f), 1, 0.25f);
        expected = new Vector2(-400f, -4f);
        Assert.That(result, Is.EqualTo(expected).Using(comparer));
    }

    [Test]
    public void CalculateCorrections_WhenCalledWithUnknownUnit_ThrowsArgumentException()
    {
       Assert.Throws<ArgumentException>(() => Ballistics.CalculateCorrections(Vector3.zero, 2, 1));
    }

    [Test]
    public void E_WhenCalled_ReturnsCorrectEnergy()
    {
        float result, expected;
        float epsilon = 1E-3f;

        result = Ballistics.E(0.001f, 700f);
        expected = 245f;
        Assert.AreEqual(expected, result, epsilon);

        result = Ballistics.E(0.014f, 890f);
        expected = 5544.7f;
        Assert.AreEqual(expected, result, epsilon);
    }

    #endregion


    #region DataSave
    
    [Test]
    public void TestSaveAndLoadDataModel()
    {
        DataModel expectedData = new()
        {
            distance = 1000f,
            windSpeed = 5f,
            windAzimuth = 45f,
            barrelElevation = 10f,
            temperature = 20f,
            atmosphericPressure = 1013.25f,
            airHumidity = 50f,
            latitude = 50f,
            aimAzimuth = 90f,
            weatherLatitude = 50f,
            weatherLongtitude = 20f
        };

        DataSave.Save(expectedData);
        DataModel actualData = DataSave.Load();

        Assert.AreEqual(expectedData.distance, actualData.distance);
        Assert.AreEqual(expectedData.windSpeed, actualData.windSpeed);
        Assert.AreEqual(expectedData.windAzimuth, actualData.windAzimuth);
        Assert.AreEqual(expectedData.barrelElevation, actualData.barrelElevation);
        Assert.AreEqual(expectedData.temperature, actualData.temperature);
        Assert.AreEqual(expectedData.atmosphericPressure, actualData.atmosphericPressure);
        Assert.AreEqual(expectedData.airHumidity, actualData.airHumidity);
        Assert.AreEqual(expectedData.latitude, actualData.latitude);
        Assert.AreEqual(expectedData.aimAzimuth, actualData.aimAzimuth);
        Assert.AreEqual(expectedData.weatherLatitude, actualData.weatherLatitude);
        Assert.AreEqual(expectedData.weatherLongtitude, actualData.weatherLongtitude);
    }
    
    #endregion


    #region WeaponSave

    [Test]
    public void TestSaveAndLoadWeaponModel()
    {
        WeaponModel expectedWeapon = new()
        {
            weaponName = "TestWeapon",
            caliber = 7.62f,
            bulletMass = 9.7f,
            ballisticCoefficient = 0.295f,
            bulletLength = 1.2f,
            threadPitch = 1f,
            barrelScopeHeight = 5.5f,
            unitsPerClick = 0.1f,
            correctionUnit = 0,
            muzzleVelocity = 800f,
            rightHandThread = true,
            zeroingDistance = 100f,
            zeroingAirPressure = 1013.25f,
            zeroingTemperature = 20f,
            zeroingHumidity = 50f,
            zeroingAimAzimuth = 0f,
            zeroingBarrelElevation = 0f,
            zeroingWindSpeed = 0f,
            zeroingWindAzimuth = 0f,
            zeroingLatitude = 0f,
            barrelAimAngleX = 0f,
            barrelAimAngleY = 0f,
            barrelAimAngleZ = 0f
        };

        WeaponSave.Save(expectedWeapon);
        var actualWeapon = WeaponSave.Load(expectedWeapon.weaponName);

        Assert.AreEqual(expectedWeapon.weaponName, actualWeapon.weaponName);
        Assert.AreEqual(expectedWeapon.caliber, actualWeapon.caliber);
        Assert.AreEqual(expectedWeapon.bulletMass, actualWeapon.bulletMass);
        Assert.AreEqual(expectedWeapon.ballisticCoefficient, actualWeapon.ballisticCoefficient);
        Assert.AreEqual(expectedWeapon.bulletLength, actualWeapon.bulletLength);
        Assert.AreEqual(expectedWeapon.threadPitch, actualWeapon.threadPitch);
        Assert.AreEqual(expectedWeapon.barrelScopeHeight, actualWeapon.barrelScopeHeight);
        Assert.AreEqual(expectedWeapon.unitsPerClick, actualWeapon.unitsPerClick);
        Assert.AreEqual(expectedWeapon.correctionUnit, actualWeapon.correctionUnit);
        Assert.AreEqual(expectedWeapon.muzzleVelocity, actualWeapon.muzzleVelocity);
        Assert.AreEqual(expectedWeapon.rightHandThread, actualWeapon.rightHandThread);
        Assert.AreEqual(expectedWeapon.zeroingDistance, actualWeapon.zeroingDistance);
        Assert.AreEqual(expectedWeapon.zeroingAirPressure, actualWeapon.zeroingAirPressure);
        Assert.AreEqual(expectedWeapon.zeroingTemperature, actualWeapon.zeroingTemperature);
        Assert.AreEqual(expectedWeapon.zeroingHumidity, actualWeapon.zeroingHumidity);
        Assert.AreEqual(expectedWeapon.zeroingAimAzimuth, actualWeapon.zeroingAimAzimuth);
        Assert.AreEqual(expectedWeapon.zeroingBarrelElevation, actualWeapon.zeroingBarrelElevation);
        Assert.AreEqual(expectedWeapon.zeroingWindSpeed, actualWeapon.zeroingWindSpeed);
        Assert.AreEqual(expectedWeapon.zeroingWindAzimuth, actualWeapon.zeroingWindAzimuth);
        Assert.AreEqual(expectedWeapon.zeroingLatitude, actualWeapon.zeroingLatitude);
        Assert.AreEqual(expectedWeapon.barrelAimAngleX, actualWeapon.barrelAimAngleX);
        Assert.AreEqual(expectedWeapon.barrelAimAngleY, actualWeapon.barrelAimAngleY);
        Assert.AreEqual(expectedWeapon.barrelAimAngleZ, actualWeapon.barrelAimAngleZ);

        WeaponSave.Delete(expectedWeapon.weaponName);
    }

    [Test]
    public void TestSaveExist()
    {
        WeaponModel weapon = new()
        {
            weaponName = "TestWeaponCheck",
            caliber = 7.62f
        };

        WeaponSave.Save(weapon);
        bool exists = WeaponSave.SaveExist(weapon.weaponName);

        Assert.IsTrue(exists);
        WeaponSave.Delete(weapon.weaponName);
    }

    [Test]
    public void TestDeleteWeaponModel()
    {
        WeaponModel weapon = new()
        {
            weaponName = "TestWeaponDelete",
            caliber = 7.62f
        };

        WeaponSave.Save(weapon);
        WeaponSave.Delete(weapon.weaponName);

        bool exists = WeaponSave.SaveExist(weapon.weaponName);
        Assert.IsFalse(exists);
    }

    [Test]
    public void TestLoadNonExistentWeaponModel()
    {
        string weaponName = "NonExistentWeapon";

        Assert.Throws<FileNotFoundException>(() => WeaponSave.Load(weaponName));
    }

    #endregion
}
