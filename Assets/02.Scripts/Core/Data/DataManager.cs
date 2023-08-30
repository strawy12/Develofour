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


        if (!Directory.Exists(D_SAVE_PATH))
        {
            D_CheckDirectory();
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

    public void CreateSaveData()
    {
        Debug.Log("세이브 크리에이트");
        saveData = new SaveData();
        saveData.isNewStart = true;
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.aiChattingList = new List<string>();
        saveData.branchPostLockData = new List<string>();
        saveData.savePhoneNumber = new List<string>();
        saveData.branchPassword = "";
        saveData.returnMonologData = new List<ReturnMonologData>();
        saveData.lastAccessDateData = new List<LastAccessDateData>();
        saveData.version = DemoVersion;
        saveData.libraryData = new List<int>();
        CreateLoginData();
        ProfilerSaveData();
        CreateGuideSaveData();
        CreateMonologData();
        CreateFileLockData();
        CreateNoticeDataSave();
        CreateProfilerGuideData();
        FileManager.Inst.ResetAdditionalFile();
        SaveToJson();

        debug_Data = saveData;
    }

    public void CreateDefaultSaveData()
    {
        defaultSaveData = new DefaultSaveData();
        defaultSaveData.BGMSoundValue = 0.6f;
        defaultSaveData.EffectSoundValue = 0.6f;
    }

    private void CreateProfilerGuideData()
    {
        saveData.profilerGuideData = new List<ProfilerGuide>();

        foreach(var guide in ResourceManager.Inst.ProfilerGuideDataSOList)
        {
            ProfilerGuide profilerGuide = new ProfilerGuide();
            profilerGuide.isAdd = guide.Value.isAddTutorial;
            profilerGuide.guideName = guide.Value.guideName;
            saveData.profilerGuideData.Add(profilerGuide);
        }
    }

    public void SaveProfilerGuideData(string name, bool flag)
    {
        foreach(var guide in saveData.profilerGuideData)
        {
            if(guide.guideName == name)
            {
                guide.isAdd = flag;
            }
        }
    }

    public void SaveBGMSoundValue(float value)
    {
        Debug.Log(defaultSaveData);
        if(defaultSaveData == null)
        {
            string data = File.ReadAllText(D_SAVE_PATH + D_SAVE_FILE);
            defaultSaveData = JsonUtility.FromJson<DefaultSaveData>(data);
        }
        defaultSaveData.BGMSoundValue = value;
    }

    public void SaveEffectSoundValue(float value)
    {
        if (defaultSaveData == null)
        {
            string data = File.ReadAllText(D_SAVE_PATH + D_SAVE_FILE);
            defaultSaveData = JsonUtility.FromJson<DefaultSaveData>(data);
        }
        defaultSaveData.EffectSoundValue = value;
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
