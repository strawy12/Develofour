using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGuideObject
{
    None,
    IncidentTab,
    CharacterTab,
    Explore,
    IncidentCategory,
    CharacterCategory,
}

public class GuideObject : MonoBehaviour
{
    [SerializeField]
    private EGuideObject objectName;
    public EGuideObject ObjectName
    {
        get => objectName;
        set => objectName = value;
    }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        EventManager.StartListening(ETutorialEvent.GuideObject, OnGuide);

        if (DataLoadingScreen.completedDataLoad == false) return;
        if (DataManager.Inst.IsPlayingProfilerTutorial() && ProfilerTutorial.guideObjectName == objectName)
        {
            OnGuide(null);
        }

    }

    private void OnGuide(object[] ps)
    {
        if (ps == null || (EGuideObject)ps[0] != objectName) return;
        if (!this.gameObject.activeSelf) return;

        GuideUISystem.OnGuide?.Invoke(transform as RectTransform);
        if(objectName == EGuideObject.CharacterTab || objectName == EGuideObject.IncidentTab)
        {
            GuideUISystem.FullSizeGuide?.Invoke(transform as RectTransform);
        }
        if (objectName == EGuideObject.CharacterCategory || objectName == EGuideObject.IncidentCategory)
        {
            GuideUISystem.CenterSizeGuide?.Invoke();
        }

        EventManager.StopListening(ETutorialEvent.GuideObject, OnGuide);
    }
}
