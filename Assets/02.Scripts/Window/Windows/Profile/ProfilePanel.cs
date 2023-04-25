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
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);

    }
    public void ChangeValue(object[] ps)
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }
        EProfileCategory category = (EProfileCategory)ps[0];
        string key = ps[1] as string;

        infoPanel.ChangeValue(category, key);
        typePanel.AddProfileCategoryPrefab(category);
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
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, ChangeValue);
    }

}
