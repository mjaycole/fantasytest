using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(object[] dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.stats";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(dataToSave);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static void SaveQuests(object[] dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/quests.stats";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(dataToSave);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.stats";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();

            return data;            
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }

    }

    public static QuestData LoadQuests()
    {
        string path = Application.persistentDataPath + "/quests.stats";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            QuestData data = (QuestData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

}
