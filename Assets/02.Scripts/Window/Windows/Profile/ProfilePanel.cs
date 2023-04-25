using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum EProfileCategory
{
    None,
    KangYohanProfile,
    ParkJuyoungProfile,
    KimYujinProfile,
    PetProfile,
    IncidentProfile,
    VictimRelationship,
    Other,
    InvisibleInformation,
    KangyohanDoubtful,
    Count,
}

public class ProfilePanel : MonoBehaviour
{
    [SerializeField]
    private ProfileInfoPanel infoPanel;
    [SerializeField]
    private ProfileCategoryTypePanel typePanel;
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
    }

    public void Show()
    {
        gameObject.SetActive(true);
        typePanel.Show();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickCharacterPanelBtn()
    {
        typePanel.ShowCharacterPanel();
    }

    private void OnClickScenePanelBtn()
    {
        typePanel.ShowScenePanel();
    }

   
}
