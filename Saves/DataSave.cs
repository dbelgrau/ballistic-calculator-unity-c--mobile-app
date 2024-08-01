using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataSave
{
    // File extension for the saved data file
    public static string dataSaveExtension = "bin";
    // Default filename for the saved data file
    public static string dataSaveFileName = "conditions";


    #region primitives

    // Method to save the DataModel object to a file
    public static void Save(DataModel data)
    {
        // Combine the persistent data path with the filename and extension to get the full file path
        string filePath = Path.Combine(Application.persistentDataPath, $"{dataSaveFileName}.{dataSaveExtension}");
        // Create a new FileStream to write the data
        using FileStream file = File.Create(filePath);
        // Serialize the DataModel object and write it to the file
        new BinaryFormatter().Serialize(file, data);
    }

    // Method to load the DataModel object from a file
    public static DataModel Load()
    {
        // Combine the persistent data path with the filename and extension to get the full file path
        string filePath = Path.Combine(Application.persistentDataPath, $"{dataSaveFileName}.{dataSaveExtension}");
        // Open the file to read the data
        using FileStream file = File.Open(filePath, FileMode.Open);
        // Deserialize the file contents into a DataModel object and return it
        return (DataModel)new BinaryFormatter().Deserialize(file);
    }

    #endregion
}
