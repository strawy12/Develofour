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

    private void CreateSaveData()
    {
        saveData = new SaveData();
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.aiChattingList = new List<AIChat>();
        saveData.branchPostLockData = new List<string>();
        saveData.savePhoneNumber = new List<string>();
        saveData.branchPassword = "";
        saveData.returnCallData = new List<ReturnCallData>();
        saveData.lastAccessDateData = new List<LastAccessDateData>();
        saveData.profilerGuideBtnSaveData = new List<string>();
        saveData.monologData = new List<MonologSaveData>();
        saveData.profilerSaveData = new List<ProfilerSaveData>();
        saveData.profilerTutorialData = new TutorialData();
        CreateLoginData();
        CreateGuideSaveData();
        CreatePinLockData();
        CreateNoticeDataSave();

        SaveToJson();

        debug_Data = saveData;
    }

    public void AddImageAiChattingList(Sprite data)
    {
        AIChat aichat = new AIChat();
        aichat.sprite = data;
        saveData.aiChattingList.Add(aichat);
    }

    public void AddTextAiChattingList(string data)
    {
        AIChat aichat = new AIChat();
        aichat.text = data;
        saveData.aiChattingList.Add(aichat);
    }

    public int AIChattingListCount()
    {
        return saveData.aiChattingList.Count;
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
