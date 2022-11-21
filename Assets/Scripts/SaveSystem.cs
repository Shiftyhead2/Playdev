using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void Save(Gamemanager gm)
    {
        BinaryFormatter bm = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gm);
        bm.Serialize(stream, data);
        stream.Close();

    }

    public static void SaveSettings(Loader loader)
    {
        BinaryFormatter bm = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Config.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(loader);
        bm.Serialize(stream, data);
        stream.Close();

    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/Config.data";
        if (File.Exists(path))
        {
            BinaryFormatter bm = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = bm.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }



    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/PlayerData.data";
        if (File.Exists(path))
        {
            BinaryFormatter bm = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

           PlayerData data =  bm.Deserialize(stream) as PlayerData;
           stream.Close();

           return data;
            
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
