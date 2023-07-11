using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStarUserListPanel : MonoBehaviour
{
    private List<OutStarCharacterDataSO> characterDataList;
    private List<OutStarUserPanel> userPanelList;
    [SerializeField]
    private Transform userPanelTransform;
    [SerializeField]
    private OutStarUserPanel userPanelPrefab;


    public void Init()
    {
        userPanelList = new List<OutStarUserPanel>();
        characterDataList = ResourceManager.Inst.GetResourceList<OutStarCharacterDataSO>();
        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);
        CreateFriendPanel();
    }
    
    private void CreateFriendPanel()
    {
        foreach(var data in characterDataList)
        {
            OutStarUserPanel userPanel = Instantiate(userPanelPrefab, userPanelTransform);
            userPanel.Init(data);
            userPanel.gameObject.SetActive(true);
            userPanelList.Add(userPanel);
        }
    }

    private void ChangeCurrentFriendData(object[] ps)
    {
        if (ps.Length < 1 || !(ps[0] is OutStarCharacterDataSO)) return;

        OutStarCharacterDataSO data = ps[0] as OutStarCharacterDataSO;
        ChangeCurrentFriendData(data);
    }

    private void ChangeCurrentFriendData(OutStarCharacterDataSO data)
    {
        userPanelList.ForEach(x => x.SetActiveSelectedImage(false));
        OutStarUserPanel userPanel = userPanelList.Find(x => x.CharacterID == data.ID);

        userPanel.SetActiveSelectedImage(true);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeCurrentFriendData);
    }
}
