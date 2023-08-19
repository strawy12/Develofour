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
    public int version;
    
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
    public List<int> libraryData;
    public bool isWatchStartCutScene;
    public bool isClearStartCutScene;
    public bool isZooglePinHintNoteOpen;
    public bool isProfilerInstall;
    // ENUM으로 타입을 나눠서 튜토리얼 타입
    // List<bool> (int)type
    public List<bool> loginData;

    public bool isNewStart = true;

    public string branchPassword;
    public bool isOnceOpenWindowProperty;

    public int CurrentTimeData;

    // 튜토리얼 중인지
    public TutorialState tutorialDataState = TutorialState.NotStart;
}

public enum TutorialState
{
    NotStart = -1, //시작 안함
    ClickIncidentInfo, //사건 이름 클릭하기 0
    ClickIncidentTab, //프로파일러 사건 탭 클릭 1
    ClickCharacterInfo, //인물 이름 클릭하기 2
    ClickCharacterTab, //프로파일러 인물 탭 클릭 3
    EndTutorial, //튜토리얼이 끝나고 전화가 옴 4
}


