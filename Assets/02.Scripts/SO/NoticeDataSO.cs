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
    public Color color;
    public string fileID;
    public ENoticeTag tag = ENoticeTag.None;
}


[CreateAssetMenu(fileName = "NoticeData", menuName = "SO/NoticeDataSO")]
public class NoticeDataSO : ScriptableObject
{
    [SerializeField]
    private ENoticeType noticeDataType;

    public NoticeData noticeData;

    public ENoticeTag noticeTag => noticeData.tag;
    public string sameTextString = "새로운 알람이 또 추가되었습니다.";
    public ENoticeType NoticeDataType => noticeDataType;
    public string Head => noticeData.head;
    public string Body => noticeData.body;
    public float Delay => noticeData.delay;
    public Sprite Icon => noticeData.icon;
    public bool CanDeleted => noticeData.canDeleted;
    public void SetNoticeData(NoticeData data)
    {
        noticeDataType = (ENoticeType)(int)ENoticeType.End;
        noticeData = data;
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
