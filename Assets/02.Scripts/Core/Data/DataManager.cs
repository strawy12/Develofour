using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private SaveData saveData;
    public SaveData SaveData => saveData;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "Data.Json";

    public SaveData debug_Data;

    private bool isInit;

    public void Init()
    {
        isInit = true;
        SAVE_PATH = Application.persistentDataPath + "/Save/";
        CheckDirectory();
        LoadFromJson();
    }

    private void CheckDirectory()
    {
        if (isInit == false) return;
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
    }

    public void CreateSaveData()
    {
        saveData = new SaveData();
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.aiChattingList = new List<string>();
        saveData.branchPostLockData = new List<string>();
        saveData.savePhoneNumber = new List<string>();
        saveData.branchPassword = "";
        saveData.returnMonologData = new List<ReturnMonologData>();
        saveData.lastAccessDateData = new List<LastAccessDateData>();

        CreateLoginData();
        ProfilerSaveData();
        CreateGuideSaveData();
        CreateMonologData();
        CreateFileLockData();
        CreateNoticeDataSave();

        SaveToJson();

        debug_Data = saveData;
    }

    public void AddAiChattingList(string data)
    {
        saveData.aiChattingList.Add(data);
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
