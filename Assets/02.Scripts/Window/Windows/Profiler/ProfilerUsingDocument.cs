using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerUsingDocument : MonoBehaviour
{
    [SerializeField]
    private Button documentsOpenBtn;
    [SerializeField]
    private Button profilerOpenBtn;

    [SerializeField]
    private GameObject usingDocuments;

    public void Init()
    {
        if (DataManager.Inst.GetProfilerTutorialState() != TutorialState.NotStart)
        {
            if(!DataManager.Inst.GetIsClearTutorial()) //튜토리얼을 못끝냈으면 
            {
                EventManager.TriggerEvent(ETutorialEvent.TutorialStart);
            }
            return;
        }
        Debug.Log("프로파일 문서 열림");
        gameObject.SetActive(true);
        usingDocuments.SetActive(false);

        documentsOpenBtn.onClick?.AddListener(DocumentsOpen);
        profilerOpenBtn.onClick?.AddListener(ProfilerOpen);
    }

    private void DocumentsOpen()
    {
        usingDocuments.SetActive(true);
    }

    private void ProfilerOpen()
    {
        profilerOpenBtn.onClick?.RemoveAllListeners();

        gameObject.SetActive(false);
        usingDocuments.SetActive(false);

        EventManager.TriggerEvent(ETutorialEvent.TutorialStart);
    }
}
