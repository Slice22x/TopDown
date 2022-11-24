using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveData(PlayerInfo Info)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "Player.Humanoid";

        FileStream Stream = new FileStream(path, FileMode.Create);

        PlayerData Data = new PlayerData(Info);
        Formatter.Serialize(Stream, Data);
        Stream.Close();
    }   

    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "Player.Humanoid";
        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            PlayerData Data = Formatter.Deserialize(Stream) as PlayerData;
            Stream.Close();
            return Data;
        }
        else
        {
            Debug.LogError("NO SAVE DATA LOCATED PLEASE MAKE NEW SAVE");
            return null;
        }
    }
}
