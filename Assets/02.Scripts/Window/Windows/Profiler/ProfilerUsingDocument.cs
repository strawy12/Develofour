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
            if(!DataManager.Inst.GetIsClearTutorial()) //Æ©Åä¸®¾óÀ» ¸ø³¡³ÂÀ¸¸é 
            {
                EventManager.TriggerEvent(ETutorialEvent.TutorialStart);
            }
            return;
        }
        if(!DataManager.Inst.SaveData.isProfilerDocument)
        {
            gameObject.SetActive(true);
            usingDocuments.SetActive(false);

            documentsOpenBtn.onClick?.AddListener(DocumentsOpen);
            profilerOpenBtn.onClick?.AddListener(ProfilerOpen);
        }
        
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
        DataManager.Inst.SaveData.isProfilerDocument = true;
        EventManager.TriggerEvent(ETutorialEvent.TutorialStart);
    }
}
