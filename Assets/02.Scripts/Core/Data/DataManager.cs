﻿using System;
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
        SAVE_PATH = Application.dataPath + "/Save/";

        CheckDirectory();
        LoadFromJson();
    }

    private void CheckDirectory()
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
    }

    private void CreatePlayerData()
    {
        playerData = new PlayerData();
        SaveToJson();
    }

    private void LoadFromJson()
    {
#if UNITY_EDITOR
        CreatePlayerData();
        Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
        return;
#endif
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            playerData = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            CreatePlayerData();
        }
    }
    private void SaveToJson()
    {
        CheckDirectory();

        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, data);
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
