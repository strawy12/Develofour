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
    public bool isLogin;
    public bool isEnterLoginSite;
    public List<ESiteLink> siteLinks = new List<ESiteLink>();
}

[System.Serializable]
public class PlayerData
{
    public ChapterData[] chapterDatas;
    public EChapterDataType currentChapterType;
    public ChapterData CurrentChapterData => chapterDatas[(int)currentChapterType];

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
