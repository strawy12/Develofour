using System.Collections;
using System.Collections.Generic;
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
}

[System.Serializable]
public class PlayerData 
{
    public ChapterData[] chapterDatas;
    public EChapterDataType currentChapterType;

    public PlayerData()
    {
        currentChapterType = EChapterDataType.Writer;
        chapterDatas = new ChapterData[(int)EChapterDataType.Count];
        chapterDatas[(int)EChapterDataType.Writer] = new WriterData();
    }

}
