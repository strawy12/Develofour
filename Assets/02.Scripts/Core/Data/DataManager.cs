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
        saveData.aiChattingList = new List<TextData>();
        saveData.loginData = new List<LoginSaveData>();

        List<FileSO> fileList = FileManager.Inst.ALLFileAddList();

        foreach (FileSO file in fileList)
        {
            if (file.isFileLock == true)
            {
                saveData.PinData.Add(new PinSaveData() { fileLocation = file.GetFileLocation(), isLock = true });
            }
        }

        for (int i = ((int)EMonologTextDataType.None) + 1; i < (int)EMonologTextDataType.Count; i++)
        {
            saveData.monologData.Add(new MonologSaveData() { monologType = (EMonologTextDataType)i, isShow = false });
        }

        for(int i = ((int)EGuideTopicName.None) + 1; i < (int)EGuideTopicName.Count; i++)
        {
            saveData.guideSaveData.Add(new GuideSaveData() { topicName = (EGuideTopicName)i, isUse = false });
        }

        for (int i = ((int)EProfileCategory.None) + 1; i < (int)EProfileCategory.Count; i++)
        {
            saveData.profileSaveData.Add(new ProfileSaveData() { category = (EProfileCategory)i,isShowCategory = false ,infoData = new List<string>() }); ;
        }
        for(int i = ((int)ELoginType.Zoogle); i < (int)ELoginType.Count; i++)
        {
            saveData.loginData.Add(new LoginSaveData() { loginType = (ELoginType)i, isLogin = false });
        }
        saveData.isStartTutorialList = InitTutorialSaveData();
        saveData.isClearTutorialList = InitTutorialSaveData();

        SaveToJson();

        debug_Data = saveData;
    }

    private List<bool> InitTutorialSaveData()
    {
        List<bool> list = new List<bool>();

        for (int i = 0; i < 3; i++)
        {
            list.Add(false);
        }

        return list;
    }
    private void LoadFromJson()
    {
#if UNITY_EDITOR
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

    public bool IsMonologShow(EMonologTextDataType type)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            Debug.Log("Json에 존재하지않는 텍스트 데이터 입니다.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(EMonologTextDataType type, bool value)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            return;
        }
        data.isShow = value;
    }

    public void AddNewFileData(FileSO file, string location)
    {
        saveData.additionFileData.Add(new AdditionFileData() { fileName = file.fileName, fileLocation = location });
    }

    public bool AdditionalFileContain(FileSO file)
    {
        AdditionFileData fileData = saveData.additionFileData.Find(x => x.fileName == file.fileName);

        if(fileData != null)
        {
            return true;
        }
        return false;
    }

    public string GetAdditionalFileName(FileSO file)
    {
        foreach (AdditionFileData data in saveData.additionFileData)
        {
            if (data.fileName.Contains(file.fileName))
            {
                return data.fileLocation;
            }
        }
        return null;
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
    public bool IsCategoryShow(EProfileCategory category)
    {
        return saveData.profileSaveData.Find(x => x.category == category).isShowCategory;
    }
    public bool IsProfileInfoData(EProfileCategory category, string str)
    {
        return GetProfileSaveData(category).infoData.Contains(str);
    }

    public bool GetIsStartTutorial(ETutorialType type)
    {
        return saveData.isStartTutorialList[(int)type];
    }

    public bool GetIsClearTutorial(ETutorialType type)
    {
        return saveData.isClearTutorialList[(int)type];
    }
    public void SetIsStartTutorial(ETutorialType type, bool value)
    {
        saveData.isStartTutorialList[(int)type] = value;
    }

    public void SetIsClearTutorial(ETutorialType type, bool value)
    {
        saveData.isClearTutorialList[(int)type] = value;
    }

    public void AddAiChattingList(TextData data)
    {
        saveData.aiChattingList.Add(data);
    }

    public bool GetIsLogin(ELoginType loginType)
    {
        return saveData.loginData.Find(x => x.loginType == loginType).isLogin;
    }

    public void SetIsLogin(ELoginType loginType, bool value)
    {
        saveData.loginData.Find(x => x.loginType == loginType).isLogin = value;
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
