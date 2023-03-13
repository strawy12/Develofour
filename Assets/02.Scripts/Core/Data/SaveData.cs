using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class FileSaveData
{
    public string fileLocation;
    public bool isLock = true;
}
[System.Serializable]
public class MonologSaveData
{
    public ETextDataType monologType;
    public bool isShow;
}

[System.Serializable]
public class SaveData
{
    public List<FileSaveData> fileData;
    public List<MonologSaveData> monologData;
    public bool isSuccessLoginZoogle;
    public bool isSuccessLoginStarbook;
    public bool isWatchStartCutScene;
}


