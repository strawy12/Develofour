using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGuideObject
{
    None,
    IncidentTab,
    CharacterTab,
    Explore,
}

public class GuideObject : MonoBehaviour
{
    [SerializeField]
    private EGuideObject objectName;
    private void Start()
    {
        EventManager.StartListening(ETutorialEvent.GuideObject, OnGuide);

        if (DataLoadingScreen.completedDataLoad == false) return;
        if (DataManager.Inst.IsProfilerTutorial() && ProfileTutorial.guideObjectName == objectName)
        {
            OnGuide(null);
        }

    }

    private void OnGuide(object[] ps)
    {
        if (ps == null || (EGuideObject)ps[0] != objectName) return;

        GuideUISystem.OnGuide?.Invoke(transform as RectTransform);
        EventManager.StopListening(ETutorialEvent.GuideObject, OnGuide);
    }
}
