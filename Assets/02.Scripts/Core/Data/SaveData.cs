using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PinLockData
{
    public string id;
    public bool isLock = true;
}

[System.Serializable]
public class MonologSaveData
{
    public string monologType;
    public bool isShow;
}
[System.Serializable]
public class AdditionFileData
{
    public string fileID;
    public string directoryID;
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
    public string categoryID;
    public bool isShowCategory;
    public List<string> infoData;
}



[System.Serializable]
public class MailSaveData
{
    [BitMask(typeof(EEmailCategory))]
    public string id;
    public int month;
    public int day;
    public int hour;
    public int minute;
}
[System.Serializable]
public class LastAccessDateData
{
    public string fileID;
    public string date;
}

[System.Serializable]
public class ReturnCallData
{
    public string id;
    [SerializeField]
    private int endDelayTime;

    public int EndDelayTime => endDelayTime;

    public void SetEndDelayTime(int delay)
    {
        endDelayTime = DataManager.Inst.GetCurrentTime() + delay;
    }
}

[System.Serializable]
public class TutorialData
{
    public bool isPlayingTutorial; // 튜토리얼을 하고있는지?
    public bool isStartTutorial; // 튜토리얼을 시작했는지?
    public bool isOverlayTutorial; // 오버레이 튜토리얼을 했는지?
    public bool isCharacterTutorial; // 인물 정보 튜토리얼을 했는지?
    public bool isIncidentTutorial; // 사건 정보 튜토리얼을 했는지?
}

[System.Serializable]
public class SaveData
{
    public List<PinLockData> PinLockData;
    public List<MonologSaveData> monologData;
    public List<AdditionFileData> additionFileData;
    public List<GuideSaveData> guideSaveData;
    public List<ProfilerSaveData> profilerSaveData;
    public List<AIChat> aiChattingList;
    public List<NoticeData> saveNoticeData;
    public List<string> branchPostLockData;
    public List<string> savePhoneNumber;
    public List<MailSaveData> mailSaveData = new List<MailSaveData>();
    public List<ReturnCallData> returnCallData;
    public List<LastAccessDateData> lastAccessDateData;
    public List<string> profilerGuideBtnSaveData;
    public TutorialData profilerTutorialData;

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
}


