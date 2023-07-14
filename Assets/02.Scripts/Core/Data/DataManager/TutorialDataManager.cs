using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{


    public bool IsStartProfilerTutorial() { return saveData.profilerTutorialData.isStartTutorial; }
    public void SetStartProfilerTutorial(bool value) { saveData.profilerTutorialData.isStartTutorial = value; }

    public bool IsPlayingProfilerTutorial() { return saveData.profilerTutorialData.isPlayingTutorial; }
    public void SetPlayingProfilerTutorial(bool value) { saveData.profilerTutorialData.isPlayingTutorial = value; }

    public void SetOverlayTutorial() { saveData.profilerTutorialData.isOverlayTutorial = true; }
    public void SetCharacterTutorial() { saveData.profilerTutorialData.isCharacterTutorial = true; }
    public void SetIncidentTutorial() { saveData.profilerTutorialData.isIncidentTutorial = true; }

    public bool GetIsClearTutorial()
    {
        return saveData.profilerTutorialData.isOverlayTutorial 
            && saveData.profilerTutorialData.isCharacterTutorial
            && saveData.profilerTutorialData.isIncidentTutorial;
    }
}
