using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class WeaponSave
{
    public static string weaponSaveExtension = "wpn";

    public static event EventHandler<EventArgs> WeaponListModified;


    #region primitives

    // Save a weapon model to a file
    public static void Save(WeaponModel weapon)
    {
        // Set the barrel aim angle before saving
        SetBarrelAimAngle(weapon);
        // Get the full file path for the weapon save file
        string filePath = Path.Combine(Application.persistentDataPath, $"{weapon.weaponName}.{weaponSaveExtension}");
        // Create a new file stream and serialize the weapon object
        using FileStream file = File.Create(filePath);
        new BinaryFormatter().Serialize(file, weapon);
        // Trigger the WeaponListModified event
        WeaponListModified?.Invoke(typeof(WeaponSave), EventArgs.Empty);
    }

    // Calculate and set the barrel aim angle for a weapon
    private static void SetBarrelAimAngle(WeaponModel weapon)
    {
        Vector3 angle = Ballistics.CalculateBarrelAimAngle(weapon);

        weapon.barrelAimAngleX = angle.x;
        weapon.barrelAimAngleY = angle.y;
        weapon.barrelAimAngleZ = angle.z;
    }

    // Load a weapon model from a file
    public static WeaponModel Load(string weaponName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{weaponName}.{weaponSaveExtension}");
        using FileStream file = File.Open(filePath, FileMode.Open);
        return (WeaponModel)new BinaryFormatter().Deserialize(file);
    }

    #endregion


    #region weapon logic

    // Check if a save file exists for a weapon
    public static bool SaveExist(string weaponName)
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, $"{weaponName}.{weaponSaveExtension}"));
    }

    // Delete a weapon save file
    public static void Delete(string weaponName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{weaponName}.{weaponSaveExtension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            WeaponListModified?.Invoke(typeof(WeaponSave), EventArgs.Empty);
        }
        else
        {
            throw new FileNotFoundException($"Could not find file '{weaponName}.{weaponSaveExtension}'");
        }
    }

    #endregion


    #region selected weapon

    // Save the selected weapon ID to PlayerPrefs
    public static void SaveSelected(int weaponId)
    {
        PlayerPrefs.SetInt("selected_weapon", weaponId);
    }

    // Load the selected weapon ID from PlayerPrefs
    public static int LoadSelected()
    {
        return PlayerPrefs.GetInt("selected_weapon", -1);
    }

    // Get a list of saved weapon names
    public static List<string> GetWeaponList()
    {
        string[] weaponSaves = Directory.GetFiles(Application.persistentDataPath, $"*.{weaponSaveExtension}");
        List<string> weapons = weaponSaves.Select(save => Path.GetFileNameWithoutExtension(save)).ToList();
        return weapons;
    }

    #endregion
}
