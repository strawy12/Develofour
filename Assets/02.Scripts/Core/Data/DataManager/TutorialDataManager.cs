using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{

    public bool IsPlayingProfilerTutorial()
    {
        return (int)TutorialState.NotStart
            < (int)saveData.tutorialDataState && (int)saveData.tutorialDataState <
            (int)TutorialState.EndTutorial;
    }

    public bool IsStartProfilerTutorial()
    {
        return GetProfilerTutorialState() != TutorialState.NotStart ? true : false;
    }

    public bool IsClearTutorial()
    {
        return saveData.tutorialDataState == TutorialState.EndTutorial;
    }

    public TutorialState GetProfilerTutorialState()
    {
        return saveData.tutorialDataState;
    }

    public void SetProfilerTutorialState(TutorialState state)
    {
        if ((int)saveData.tutorialDataState > (int)state) return; //현재의 스테이트보다 과거의 스테이트가 올때
        saveData.tutorialDataState = state;
    }

}