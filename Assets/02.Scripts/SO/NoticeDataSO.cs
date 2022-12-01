using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ENoticeDataType
{
    None = -1,
    Youtube = 0,
    Login,
    Gmali,
    Police,
    CheckGmail,
    Blog,
}

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
    private ENoticeDataType noticeDataType;

    [SerializeField]
    private NoticeData noticeDataList;

    public ENoticeDataType NoticeDataType
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
}
