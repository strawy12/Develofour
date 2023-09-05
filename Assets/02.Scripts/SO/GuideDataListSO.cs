using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuideData
{
    public EGuideTopicName topicName;
    public string monologID;
}

[CreateAssetMenu(menuName ="SO/GuideData")]
public class GuideDataListSO : ScriptableObject
{
    public List<GuideData> guideDataList;
}
