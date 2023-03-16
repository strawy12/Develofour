using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PinSaveData
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
public class AdditionFileData
{
    public string fileLocation;
}
public class GuideSaveData
{
    public EGuideTopicName topicName;
    public bool isUse;
}

[System.Serializable]
public class SaveData
{
    public List<PinSaveData> PinData;
    public List<MonologSaveData> monologData;
    public List<AdditionFileData> additionFileData;
    public List<GuideSaveData> guideSaveData;
    public bool isSuccessLoginZoogle;
    public bool isSuccessLoginStarbook;
    public bool isWatchStartCutScene;
    public bool isClearStartCutScene;
    public bool isZooglePinHintNoteOpen;
    public EComputerLoginState computerLoginState;
}


