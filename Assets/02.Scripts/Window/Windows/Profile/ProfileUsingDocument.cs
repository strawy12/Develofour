using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUsingDocument : MonoBehaviour
{
    [SerializeField]
    private Button documentsOpenBtn;
    [SerializeField]
    private Button profilerOpenBtn;

    [SerializeField]
    private GameObject usingDocuments;

    public void Init()
    {
        if (DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) == true)
        {
            return;
        }

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
        DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);
    }
}
