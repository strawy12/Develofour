﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum EProfileCategory
{
    None,
    WomanKidnapping,
    AssaultBruised,
    VictimStalkingInfo,
    TalkingFightInfo,
    PetCamVideo,
    VictimBodyInspection,
    CctvKidnappingVideo,
    VictimLastCalling,
    MentalDrugInfo,
    SuspectAndAccompliceChat,
    MurderWeaponInfo,
    CorpseLocationGroveInfo,
    DieToDogForDiaryInfo,
    MonoLogInformation,
    InvisibleInformation,
    Count,
}

public class ProfilePanel : MonoBehaviour
{
    public ProfileInfoPanel infoPanel;

    [SerializeField]
    private ProfileInventoryPanel typePanel;
    [SerializeField]
    private Sprite profilerSpeite;
    [SerializeField]
    private Button sceneBtn;
    [SerializeField]
    private Button characterBtn;

    public void Init()
    {
        infoPanel.Init();
        typePanel.Init();
        characterBtn.onClick.AddListener(OnClickCharacterPanelBtn);
        sceneBtn.onClick.AddListener(OnClickScenePanelBtn);
        EventManager.StartListening(EProfileEvent.FindInfoInProfile, ChangeValue);

        Show();

    }
    public void ChangeValue(object[] ps)
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }
        EProfileCategory category = (EProfileCategory)ps[0];
        string key = ps[1] as string;

        typePanel.AddProfileCategoryPrefab(category);
        infoPanel.ChangeValue(category, key);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        typePanel.Show();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        typePanel.Hide();
    }

    private void OnClickCharacterPanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfileCategoryType.Character) == false)
            typePanel.ShowCharacterPanel();
    }

    private void OnClickScenePanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfileCategoryType.Scene) == false)
            typePanel.ShowScenePanel();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoInProfile, ChangeValue);
    }

}
