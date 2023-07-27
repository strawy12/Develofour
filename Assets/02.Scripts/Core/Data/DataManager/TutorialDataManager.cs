using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public bool IsStartProfilerTutorial() { return saveData.profilerTutorialData.isStartTutorial; }
    public void SetStartProfilerTutorial(bool value) { saveData.profilerTutorialData.isStartTutorial = value; }
    public bool IsCallTutorial() { return saveData.profilerTutorialData.isCanCallTutorial; }
    public void SetCallTutorial(bool value) { saveData.profilerTutorialData.isCanCallTutorial = value; }

    public bool IsPlayingProfilerTutorial() { return saveData.profilerTutorialData.isPlayingTutorial; }
    public void SetPlayingProfilerTutorial(bool value) { saveData.profilerTutorialData.isPlayingTutorial = value; }

    public bool IsClearTutorial() { return saveData.profilerTutorialData.isClearTutorial; }
    public void SetIsClearTutorial(bool value) { saveData.profilerTutorialData.isClearTutorial = value; }
}
