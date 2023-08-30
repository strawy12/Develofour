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


    private DefaultSaveData defaultSaveData;
    public DefaultSaveData DefaultSaveData => defaultSaveData;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "Data.Json";

    private string D_SAVE_PATH = "";
    private const string D_SAVE_FILE = "DefaultData.Json";

    public SaveData debug_Data;
    public int DemoVersion = 1;
    private bool isInit;

    void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save/";
        D_SAVE_PATH = Application.persistentDataPath + "/Save/Default/";
    }

    public void Init()
    {
        isInit = true;

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

    public void D_CheckDirectory()
    {

        if (!Directory.Exists(D_SAVE_PATH))
        {
            Directory.CreateDirectory(D_SAVE_PATH);
            CreateDefaultSaveData();
        }
    }


    private void CreateSaveData()
    {
        saveData = new SaveData();
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.aiChattingList = new List<AIChat>();
        saveData.branchPostLockData = new List<string>();
        saveData.libraryData = new List<string>();
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

    public void CreateDefaultSaveData()
    {
        defaultSaveData = new DefaultSaveData();
        defaultSaveData.BGMSoundValue = 0.6f;
        defaultSaveData.EffectSoundValue = 0.6f;
    }

    public void AddImageAiChattingList(Sprite data)
    {
        AIChat aichat = new AIChat();
        aichat.sprite = data;
        saveData.aiChattingList.Add(aichat);
        Debug.Log(saveData.aiChattingList.Count);
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
