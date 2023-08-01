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
        if (!(ps[0] is OutStarProfileDataSO)) { return; }
        OutStarProfileDataSO characterData = ps[0] as OutStarProfileDataSO;
        
        ChangeUserData(characterData);
    }

    private void ChangeUserData(OutStarProfileDataSO data)
    {
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(data.ID);
        userNameText.SetText(characterData.characterName);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeUserChatingData);
    }
}
