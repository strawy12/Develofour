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

    public bool canDeleted = true;

    public Sprite icon;
}


[CreateAssetMenu(fileName = "NoticeData", menuName = "SO/NoticeDataSO")]
public class NoticeDataSO : ScriptableObject
{
    [SerializeField]
    private ENoticeType noticeDataType;

    [SerializeField]
    private NoticeData noticeDataList;

    public ENoticeTag noticeTag = ENoticeTag.None;
    public string sameTextString = "새로운 알람이 또 추가되었습니다.";
    public ENoticeType NoticeDataType => noticeDataType;
    public string Head => noticeDataList.head;
    public string Body => noticeDataList.body;
    public float Delay => noticeDataList.delay;
    public Sprite Icon => noticeDataList.icon;
    public bool CanDeleted => noticeDataList.canDeleted;

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
