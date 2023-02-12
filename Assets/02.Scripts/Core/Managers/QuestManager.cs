using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{
    [SerializeField]
    private Transform quests;

    private List<Quest> questList;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        questList = new List<Quest>();

        for (int i = 0; i < quests.childCount; i++)
        {
            Quest quest = quests.GetChild(i).GetComponent<Quest>();
            questList.Add(quest);
            quest.gameObject.SetActive(false);
        }
        CheckClearQuest();
        CheckActiveQuest();
    }

    private void CheckClearQuest()
    {
        List<Quest> quests = questList;
        
        foreach (Quest quest in quests)
        {
            if (quest.QuestData.isClear == true)
            {
                questList.Remove(quest);
            }
        }
    }

    private void CheckActiveQuest()
    {
        foreach (Quest quest in questList)
        {
            if (quest.QuestData.isActive)
            {
                quest.gameObject.SetActive(true);
                quest.Init(false);
            }
        }
    }

    public void AddQuest(EQuestEvent questEvent, bool isNotice)
    {
        foreach(Quest quest in questList)
        {
            if(quest.QuestData.questEvent == questEvent)
            {
                if(!quest.QuestData.isActive)
                {
                    quest.gameObject.SetActive(true);
                    quest.Init(isNotice);

                    //??
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        questList.ForEach(x => x.DebugReset());
    }
}
