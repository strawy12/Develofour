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

    public void Init()
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

    private void CreateSaveData()
    {
        saveData = new SaveData();
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.aiChattingList = new List<TextData>();
        saveData.branchPostLockData = new List<string>();
        saveData.savePhoneNumber = new List<string>();
        saveData.branchPassword = "";
        CreateTutorialList();
        CreateLoginData();
        ProfileSaveData();
        CreateGuideSaveData();
        CreateMonologData();
        CreateFileLockData();
        CreateNoticeDataSave();
        CreateMailData();
        SaveToJson();

        debug_Data = saveData;
    }

    public void AddAiChattingList(TextData data)
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
