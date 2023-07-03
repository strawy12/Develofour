using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChattingPanel : MonoBehaviour
{
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private UserChattingPanel userChattingPanelTemp;
    private OutStarCharacterDataSO currentCharacterData;

    public void Init()
    {
        userChattingPanelTemp.Init();

        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeUserChatingData);
    }

    private void ChangeUserChatingData(object[] ps)
    {
        if (!(ps[0] is OutStarCharacterDataSO)) { return; }
        OutStarCharacterDataSO characterData = ps[0] as OutStarCharacterDataSO;

        ChangeUserData(characterData);

    }

    private void ChangeUserData(OutStarCharacterDataSO data)
    {
        currentCharacterData = data;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeUserChatingData);
    }
}
