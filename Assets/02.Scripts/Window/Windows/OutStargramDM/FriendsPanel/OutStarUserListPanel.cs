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
        characterDataList = ResourceManager.Inst.GetOutStarCharacterDataToList();
        CreateFriendPanel();
        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);

    }
    
    private void CreateFriendPanel()
    {
        foreach(var data in characterDataList)
        {
            OutStarUserPanel userPanel = Instantiate(friendPanelPrefab, transform);
            userPanel.Init(data);
            userPanel.gameObject.SetActive(true);
            userPanelList.Add(userPanel);
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
        userPanelList.ForEach(x => x.SetActiveSelectedImage(false));
        OutStarUserPanel userPanel = userPanelList.Find(x => x.CharacterID == data.ID);

        currentData = data;
        userPanel.SetActiveSelectedImage(true);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);
    }
}
