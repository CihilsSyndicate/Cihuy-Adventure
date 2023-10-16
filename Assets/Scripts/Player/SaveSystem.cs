using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerMovement player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.njir";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Player.njir";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSavedData()
    {
        string path = Application.persistentDataPath + "/ChestData.njir"; // Ganti nama file sesuai kebutuhan
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Saved data deleted.");
        }
        else
        {
            Debug.Log("No saved data found.");
        }
    }


    public static void SaveChestData(ChestData chestData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ChestData.njir"; // Ganti nama file sesuai kebutuhan
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, chestData);
        stream.Close();
    }

    public static ChestData LoadChestData()
    {
        string path = Application.persistentDataPath + "/ChestData.njir"; // Ganti nama file sesuai kebutuhan
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ChestData chestData = formatter.Deserialize(stream) as ChestData;
            stream.Close();

            return chestData;
        }
        else
        {
            // Jika file tidak ditemukan, kembalikan objek baru
            Debug.LogWarning("KONZ");
            return null;
        }
    }
}