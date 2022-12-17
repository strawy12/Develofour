using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum EChapterDataType
{
    None = -1,
    Writer,
    Count
}

[System.Serializable]
public class WriterData : ChapterData
{
}


[System.Serializable]
public class ChapterData
{
    public bool isWindowLogin;
    public bool isEnterLoginGoogleSite;
    public List<ESiteLink> siteLinks;
    public bool isLoginSNSSite;
    public string SNSPassword;

    public ChapterData()
    {
        siteLinks = new List<ESiteLink>();
        siteLinks.Add(ESiteLink.Youtube_News);
        isEnterLoginGoogleSite = false;
        isWindowLogin = false;
        isLoginSNSSite = false;
    }
}

public class QuestClearData
{
    public bool isPoliceMiniGameClear;
}

[System.Serializable]
public class PlayerData
{
    public ChapterData[] chapterDatas;
    public EChapterDataType currentChapterType;
    public ChapterData CurrentChapterData => chapterDatas[(int)currentChapterType];
    public QuestClearData questClearData;
    public PlayerData()
    {
        currentChapterType = EChapterDataType.Writer;
        chapterDatas = new ChapterData[(int)EChapterDataType.Count];
        chapterDatas[(int)EChapterDataType.Writer] = new WriterData();
    }

    public T TypeCastChapterData<T>() where T : ChapterData
    {
        return CurrentChapterData as T;
    }
}