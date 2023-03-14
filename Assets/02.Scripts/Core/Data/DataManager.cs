using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    private static SaveData saveData;

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
        saveData.pinLockData = new List<PinLockData>();

        List<FileSO> fileList = FileManager.Inst.ALLFileAddList();

        foreach(FileSO file in fileList)
        {
            if(file.isFileLock == true)
            {
                saveData.pinLockData.Add(new PinLockData() { fileLocation = file.GetFileLocation(), isLock = true });
            }
        }

        SaveToJson();

        debug_Data = saveData;
    }

    private void LoadFromJson()
    {
#if UNITY_EDITOR
        CreateSaveData();
        Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
        return;
#endif
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            CreateSaveData();
        }
    }
    private void SaveToJson()
    {
        CheckDirectory();

        Debug.Log("SaveToJson");

        string data = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, data);
    }
    public static T GetSaveData<T>(ESaveDataType fieldType)
    {
        string fieldName = fieldType.ToString();
        fieldName = char.ToLower(fieldName[0]) + fieldName.Substring(1);

        FieldInfo info = saveData.GetType().GetField(fieldName);
        T value = (T)info.GetValue(saveData);
        return value;
    }

    public static void SetSaveData<T>(ESaveDataType fieldType, T value)
    {
        string fieldName = fieldType.ToString();
        fieldName = char.ToLower(fieldName[0]) + fieldName.Substring(1);

        FieldInfo info = saveData.GetType().GetField(fieldName);
        info.SetValue(saveData, value);
    }

    public static bool IsWindowLock(string fileLocation)
    {
        PinLockData data = saveData.pinLockData.Find(x => x.fileLocation == fileLocation);
        if(data == null)
            return true;

        return data.isLock;
    }

    public static void SetWindowLock(string fileLocation, bool value)
    {
        PinLockData data = saveData.pinLockData.Find(x => x.fileLocation == fileLocation);
        if (data != null)
            data.isLock = value;
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
