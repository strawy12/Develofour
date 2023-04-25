using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private List<bool> InitTutoList()
    {
        List<bool> list = new List<bool>();

        for(int i =0; i < (int)ETutorialType.Count; i++)
        {
            list.Add(false);
        }
        return list;
    }

    private void CreateTutorialList()
    {
        saveData.isStartTutorialList = InitTutoList();
        saveData.isClearTutorialList = InitTutoList();
    }

    public bool GetIsStartTutorial(ETutorialType type)
    {
        return saveData.isStartTutorialList[(int)type];
    }

    public bool GetIsClearTutorial(ETutorialType type)
    {
        return saveData.isClearTutorialList[(int)type];
    }

    public void SetIsStartTutorial(ETutorialType type, bool value)
    {
        saveData.isStartTutorialList[(int)type] = value;
    }

    public void SetIsClearTutorial(ETutorialType type, bool value)
    {
        saveData.isClearTutorialList[(int)type] = value;
    }
}
