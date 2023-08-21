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
        if (DataManager.Inst.IsProfilerTutorial() && ProfilerTutorial.guideObjectName == objectName)
        {
            OnGuide(new object[] { objectName });
        }

    }

    private void OnGuide(object[] ps)
    {
        if (ps == null || (EGuideObject)ps[0] != objectName) return;
        if (transform == null) return;
        GuideUISystem.OnGuide?.Invoke(transform as RectTransform);
        if(objectName == EGuideObject.CharacterTab || objectName == EGuideObject.IncidentTab)
        {
            GuideUISystem.OnFullSizeGuide?.Invoke(transform as RectTransform);
        }
        EventManager.StopListening(ETutorialEvent.GuideObject, OnGuide);
    }

    void OnDestroy()
    {
        EventManager.StopListening(ETutorialEvent.GuideObject, OnGuide);
    }

}
