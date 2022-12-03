using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoticeData
{
    [TextArea(1, 3)]
    public string head;
    [TextArea(5, 30)]
    public string body;

    public float delay;
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

    public void SetNoticeData(NoticeData data)
    {
        noticeDataList = data;
    }    
}
