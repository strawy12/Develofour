using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class ChattingPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text userNameText;
    
    public void Init()
    {
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
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetCharacterDataSO(data.ID);
        userNameText.SetText(characterData.characterName);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeUserChatingData);
    }
}
