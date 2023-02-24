using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/SaveNoticeData")]
public class SaveNoticeDataSO : ScriptableObject
{
    public List<NoticeData> noticeDataList = new List<NoticeData>();
}
