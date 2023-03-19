using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    private SaveData saveData;
    public SaveData SaveData => saveData;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "Data.Json";

    public SaveData debug_Data;

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

    private void CreateSaveData()
    {
        saveData = new SaveData();
        saveData.PinData = new List<PinSaveData>();
        saveData.monologData = new List<MonologSaveData>();
        saveData.additionFileData = new List<AdditionFileData>();
        saveData.guideSaveData = new List<GuideSaveData>();
        saveData.profileSaveData = new List<ProfileSaveData>();
        List<FileSO> fileList = FileManager.Inst.ALLFileAddList();

        foreach (FileSO file in fileList)
        {
            if (file.isFileLock == true)
            {
                saveData.PinData.Add(new PinSaveData() { fileLocation = file.GetFileLocation(), isLock = true });
            }
        }

        for (int i = ((int)ETextDataType.None) + 1; i < (int)ETextDataType.Count; i++)
        {
            saveData.monologData.Add(new MonologSaveData() { monologType = (ETextDataType)i, isShow = false });
        }

        for(int i = ((int)EGuideTopicName.None) + 1; i < (int)EGuideTopicName.Count; i++)
        {
            saveData.guideSaveData.Add(new GuideSaveData() { topicName = (EGuideTopicName)i, isUse = false });
        }

        for (int i = ((int)EProfileCategory.None) + 1; i < (int)EProfileCategory.Count; i++)
        {
            saveData.profileSaveData.Add(new ProfileSaveData() { category = (EProfileCategory)i,isShowCategory = false ,infoData = new List<string>() }); ;
        }
        SaveToJson();

        debug_Data = saveData;
    }

    private void LoadFromJson()
    {
        #if   UNITY_EDITOR
                CreateSaveData();
                Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
                return;
        #else
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            CreateSaveData();
        }
        #endif
    }
    private void SaveToJson()
    {
        CheckDirectory();

        Debug.Log("SaveToJson");

        string data = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, data);
    }

    public bool IsWindowLock(string fileLocation)
    {
        PinSaveData data = saveData.PinData.Find(x => x.fileLocation == fileLocation);
        if (data == null)
            return true;

        return data.isLock;
    }

    public void SetWindowLock(string fileLocation, bool value)
    {
        PinSaveData data = saveData.PinData.Find(x => x.fileLocation == fileLocation);
        if (data != null)
            data.isLock = value;
    }

    public bool IsMonologShow(ETextDataType type)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            Debug.Log("Json에 존재하지않는 텍스트 데이터 입니다.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(ETextDataType type, bool value)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            return;
        }
        data.isShow = value;
    }

    public void AddNewFileData(string location)
    {
        saveData.additionFileData.Add(new AdditionFileData() { fileLocation = location });
    }

    public bool AdditionalFileContain(string location)
    {
        foreach(AdditionFileData data in saveData.additionFileData)
        {
            if(data.fileLocation == location)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsGuideUse(EGuideTopicName topicName)
    {
        GuideSaveData guideData = saveData.guideSaveData.Find(x => x.topicName == topicName);
        if(guideData == null)
        {
            return true;
        }
        return guideData.isUse;
    }
    public void SetGuide(EGuideTopicName topicName, bool value)
    {
        GuideSaveData guideData = saveData.guideSaveData.Find(x => x.topicName == topicName);
        if (guideData == null)
        {
            return;
        }
        guideData.isUse = value;
    }

    public ProfileSaveData GetProfileSaveData(EProfileCategory category)
    {
        ProfileSaveData data = saveData.profileSaveData.Find(x => x.category == category);
        return data;
    }

    public void AddProfileinfoData(EProfileCategory category, string key)
    {
        if (GetProfileSaveData(category).infoData.Contains(key))
        {
            return;
        }
        saveData.profileSaveData.Find(x => x.category == category).infoData.Add(key);
    }

    public void SetCategoryData(EProfileCategory category, bool value)
    {
        saveData.profileSaveData.Find(x => x.category == category).isShowCategory = value;
    }

    public bool IsProfileInfoData(EProfileCategory category, string str)
    {
        return GetProfileSaveData(category).infoData.Contains(str);
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
