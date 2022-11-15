using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField]
    private PlayerData playerData;
    public PlayerData CurrentPlayer => playerData;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "Data.Json";

    private void Awake()
    {
        //Application.persistentDataPath : 사용자디렉토리 / AppData / LocalLow / 회사이름 / 프로덕트이름
        SAVE_PATH = Application.persistentDataPath + "/saves/";
    }

    private void CheckDirectory()
    {
        if(!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
    }

    private void CreatePlayerData()
    {
        playerData = new PlayerData();
    }

    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            playerData = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            CreatePlayerData();
        }

        SaveToJson();
    }

    private void SaveToJson()
    {
        CheckDirectory();

        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(SAVE_PATH, data);
    }

    private void OnDestroy()
    {
        SaveToJson();
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
