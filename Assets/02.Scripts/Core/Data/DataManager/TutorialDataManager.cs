using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{

    public bool IsProfilerTutorial()
    {
        return 0 <= saveData.tutorialDataIdx && saveData.tutorialDataIdx <= 4;
    }
    
    public int GetProfileTutorialIdx()
    {
        return saveData.tutorialDataIdx;
    }

    public void SetProfilerTutorialIdx()
    {
        if (saveData.tutorialDataIdx > 4) return;
        saveData.tutorialDataIdx++;
    }

    public bool GetIsClearTutorial()
    {
        return saveData.tutorialDataIdx > 4;
    }
}
