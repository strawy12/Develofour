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

public enum TutorialState
{
    NotStart = -1, //시작 안함
    ClickIncidentInfo, //사건 이름 클릭하기 0
    ClickIncidentTab, //프로파일러 사건 탭 클릭 1
    ClickCharacterInfo, //인물 이름 클릭하기 2
    ClickCharacterTab, //프로파일러 인물 탭 클릭 3
    Overlay, // 오버레이 튜토리얼 모든 정보 획득 4
    FileLock, // 파일 락 해금 해야함 5
    EndTutorial, //튜토리얼이 끝나고 전화가 오지만 일단는 전화는 안함
}

[System.Serializable]
public class SaveData
{
    public int version = 1;
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
    public List<string> libraryData;
    public bool isWatchStartCutScene;
    public bool isClearStartCutScene;
    public bool isZooglePinHintNoteOpen;
    public bool isProfilerInstall;
    public bool isOutStarLogin;
    public bool isNewStart = true;
    // ENUM으로 타입을 나눠서 튜토리얼 타입
    // List<bool> (int)type
    public List<bool> loginData;

    public string branchPassword;
    public bool isOnceOpenWindowProperty;

    public TutorialState tutorialDataState = TutorialState.NotStart;

    public int CurrentTimeData;
}

public class DefaultSaveData
{
    public float BGMSoundValue = 0.6f;
    public float EffectSoundValue = 0.6f;
    //윈도우 크기? 전체화면?
}


