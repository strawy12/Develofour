using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NoticeData
{
    [TextArea(1, 3)]
    public string head;
    [TextArea(5, 30)]
    public string body;

    public float delay;

    public Sprite icon;
}


[CreateAssetMenu(fileName = "NoticeData", menuName = "SO/NoticeDataSO")]
public class NoticeDataSO : ScriptableObject
{
    [SerializeField]
    private ENoticeType noticeDataType;

    [SerializeField]
    private NoticeData noticeDataList;

    public ENoticeType NoticeDataType
    {
        get
        {
            return noticeDataType;
        }
    }

    public string Head
    {
        get
        {
            return noticeDataList.head;
        }
    }

    public string Body
    {
        get
        {
            return noticeDataList.body;
        }
    }

    public float Delay
    {
        get
        {
            return noticeDataList.delay;
        }
    }

    public Sprite Icon
    {
        get
        {
            return noticeDataList.icon;
        }
    }

    public void SetNoticeData(NoticeData data)
    {
        noticeDataType = (ENoticeType)(int)ENoticeType.End;
        noticeDataList = data;
    }

#if UNITY_EDITOR
    [ContextMenu("SetSOName")]
    public void SetSOName()
    {
        string SO_PATH = $"Assets/Resources/NoticeData/NoticeData_{noticeDataType.ToString()}.asset";
        
        AssetDatabase.CreateAsset(this, SO_PATH);
    }
#endif

}
