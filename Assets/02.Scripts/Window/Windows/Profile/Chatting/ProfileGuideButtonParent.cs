using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButtonParent : MonoBehaviour
{
    [SerializeField]
    private ProfileGuideButton guideButtonPrefab;

    [SerializeField]
    private Transform poolParent;

    private List<ProfileGuideButton> guideButtonList;

    private Queue<ProfileGuideButton> poolQueue;

    public void Init()
    {
        guideButtonList = new List<ProfileGuideButton>();
        CreatePool();
    }
    #region Pool
    private void CreatePool()
    {
        for(int i = 0; i < 15; i++)
        {
            ProfileGuideButton button = Instantiate(guideButtonPrefab, poolParent);

            button.gameObject.SetActive(false);
            poolQueue.Enqueue(button);
        }
    }

    private ProfileGuideButton PopButton()
    {
        if(poolQueue.Count <= 0)
        {
            CreatePool();
        }

        return poolQueue.Dequeue();
    }

    private void PushButton(ProfileGuideButton button)
    {
        guideButtonList.Remove(button);
        button.gameObject.SetActive(false);
        poolQueue.Enqueue(button);
    }
    #endregion

    public void AddButton(EProfileCategory category, string infoKey)
    {

    }

}
