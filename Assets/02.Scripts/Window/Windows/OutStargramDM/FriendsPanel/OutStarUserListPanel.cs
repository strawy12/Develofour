using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStarUserListPanel : MonoBehaviour
{
    private List<OutStarCharacterDataSO> characterDataList;
    private List<OutStarUserPanel> userPanelList;
    [SerializeField]
    private OutStarUserPanel friendPanelPrefab;

    private OutStarCharacterDataSO currentData;

    public void Init()
    {
        //characterDataList = ResourceManager.Inst
        CreateFriendPanel();
        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);

    }
    
    private void CreateFriendPanel()
    {
        foreach(var data in characterDataList)
        {
            OutStarUserPanel friendPanel = Instantiate(friendPanelPrefab, transform);
            friendPanel.Init(data);
            friendPanel.gameObject.SetActive(true);
            userPanelList.Add(friendPanel);
        }
    }
    
    private void ChangeCurrentFriendData(object[] ps)
    {
        if(!(ps[0] is OutStarCharacterDataSO)) return;

        OutStarCharacterDataSO data = ps[0] as OutStarCharacterDataSO;
        ChangeCurrentFriendData(data);
    }

    private void ChangeCurrentFriendData(OutStarCharacterDataSO data)
    {
        OutStarUserPanel userPanel = userPanelList.Find(x => x.CharacterID == data.ID);

        currentData = data;
        //userPanel.
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);
    }
}
