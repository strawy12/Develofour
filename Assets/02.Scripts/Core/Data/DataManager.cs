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
        saveData.pinLockData = new List<PinLockData>();
        saveData.monologSaveData = new List<MonologSaveData>();
        List<FileSO> fileList = FileManager.Inst.ALLFileAddList();

        foreach (FileSO file in fileList)
        {
            if (file.isFileLock == true)
            {
                saveData.pinLockData.Add(new PinLockData() { fileLocation = file.GetFileLocation(), isLock = true });
            }
        }

        for (int i = ((int)ETextDataType.None) + 1; i < (int)ETextDataType.Count; i++)
        {
            saveData.monologSaveData.Add(new MonologSaveData() { monologType = (ETextDataType)i, isShow = false });
        }

        SaveToJson();

        debug_Data = saveData;
    }

    private void LoadFromJson()
    {
        //#if   
        //        CreateSaveData();
        //        Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
        //        return;
        //#endif
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

    public bool IsWindowLock(string fileLocation)
    {
        PinLockData data = saveData.pinLockData.Find(x => x.fileLocation == fileLocation);
        if (data == null)
            return true;

        return data.isLock;
    }

    public void SetWindowLock(string fileLocation, bool value)
    {
        PinLockData data = saveData.pinLockData.Find(x => x.fileLocation == fileLocation);
        if (data != null)
            data.isLock = value;
    }

    public bool IsMonologShow(ETextDataType type)
    {
        MonologSaveData data = saveData.monologSaveData.Find(x => x.monologType == type);
        if (data == null)
        {
            Debug.Log("Json에 존재하지않는 텍스트 데이터 입니다.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(ETextDataType type, bool value)
    {
        MonologSaveData data = saveData.monologSaveData.Find(x => x.monologType == type);
        if (data == null)
        {
            return;
        }
        data.isShow = value;
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
