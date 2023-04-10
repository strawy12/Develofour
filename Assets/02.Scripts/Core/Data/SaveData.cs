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
    public EMonologTextDataType monologType;
    public bool isShow;
}
[System.Serializable]
public class AdditionFileData
{
    public string fileName;
    public string fileLocation;
}
[System.Serializable]
public class GuideSaveData
{
    public EGuideTopicName topicName;
    public bool isUse;
}
[System.Serializable]
public class ProfileSaveData
{
    public EProfileCategory category;
    public bool isShowCategory;
    public List<string> infoData;
}

[System.Serializable]
public class SaveData
{
    public List<PinSaveData> PinData;
    public List<MonologSaveData> monologData;
    public List<AdditionFileData> additionFileData;
    public List<GuideSaveData> guideSaveData;
    public List<ProfileSaveData> profileSaveData;
    public List<TextData> aiChattingList;
    
    public bool isSuccessLoginZoogle;
    public bool isSuccessLoginStarbook;
    public bool isWatchStartCutScene;
    public bool isClearStartCutScene;
    public bool isZooglePinHintNoteOpen;
    public bool isProfilerInstall;
    public bool isSuccessLoginHarmony;

    // ENUM으로 타입을 나눠서 튜토리얼 타입
    // List<bool> (int)type
    public List<bool> isStartTutorialList;
    public List<bool> isClearTutorialList;

    public bool isOnceOpenWindowProperty;
    public bool isOnceEnterZoogleLogin;

    public EComputerLoginState computerLoginState;
}


