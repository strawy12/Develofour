using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class FileLockData
{
    public int id;
    public bool isLock = true;
}

[System.Serializable]
public class MonologSaveData
{
    public int monologType;
    public bool isShow;
}
[System.Serializable]
public class AdditionFileData
{
    public int fileID;
    public int directoryID;
}
[System.Serializable]
public class GuideSaveData
{
    public EGuideTopicName topicName;
    public bool isUse;
}


[System.Serializable]
public class ProfilerSaveData
{
    public EProfilerCategory category;
    public bool isShowCategory;
    public List<int> infoData;
}



[System.Serializable]
public class MailSaveData
{
    [BitMask(typeof(EEmailCategory))]
    public int id;
    public int month;
    public int day;
    public int hour;
    public int minute;
}
[System.Serializable]
public class LastAccessDateData
{
    public int fileID;
    public string date;
}
[System.Serializable]
public class SaveData
{
    public List<FileLockData> FileLockData;
    public List<MonologSaveData> monologData;
    public List<AdditionFileData> additionFileData;
    public List<GuideSaveData> guideSaveData;
    public List<ProfilerSaveData> profilerSaveData;
    public List<string> aiChattingList;
    public List<NoticeData> saveNoticeData;
    public List<string> branchPostLockData;
    public List<string> savePhoneNumber;
    public List<MailSaveData> mailSaveData = new List<MailSaveData>();
    public List<ReturnMonologData> returnMonologData;
    public List<LastAccessDateData> lastAccessDateData;
    public List<string> profilerGuideBtnSaveData;
    public bool isWatchStartCutScene;
    public bool isClearStartCutScene;
    public bool isZooglePinHintNoteOpen;
    public bool isProfilerInstall;
    public bool isOutStarLogin;
    // ENUM으로 타입을 나눠서 튜토리얼 타입
    // List<bool> (int)type
    public List<bool> loginData;

    public string branchPassword;
    public bool isOnceOpenWindowProperty;

    public int CurrentTimeData;

    // 튜토리얼 중인지
    public int tutorialDataIdx = -1;
}


