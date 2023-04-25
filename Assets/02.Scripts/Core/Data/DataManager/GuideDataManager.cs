using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateGuideSaveData()
    {
        saveData.guideSaveData = new List<GuideSaveData>();
        for (int i = ((int)EGuideTopicName.None) + 1; i < (int)EGuideTopicName.Count; i++)
        {
            saveData.guideSaveData.Add(new GuideSaveData() { topicName = (EGuideTopicName)i, isUse = false });
        }
    }

    public bool IsGuideUse(EGuideTopicName topicName)
    {
        GuideSaveData guideData = saveData.guideSaveData.Find(x => x.topicName == topicName);
        if (guideData == null)
        {
            return true;
        }
        return guideData.isUse;
    }
    public void SetGuide(EGuideTopicName topicName, bool value)
    {
        GuideSaveData guideData = saveData.guideSaveData.Find(x => x.topicName == topicName);
        if (guideData == null)
        {
            return;
        }
        guideData.isUse = value;
    }
}
