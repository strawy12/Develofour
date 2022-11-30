using UnityEngine;

[System.Serializable]
public struct NoticeData
{
    public string head;
    public string body;
    public float delay;
}

public class NoticeDataSO : ScriptableObject
{
    public NoticeData noticeData;
}
