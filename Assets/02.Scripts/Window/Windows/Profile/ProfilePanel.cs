using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        sceneBtn.onClick.AddListener(OnClickIncidentPanelBtn);
        EventManager.StartListening(EProfileEvent.FindInfoInProfile, ChangeValue);

        Show();

    }
    public void ChangeValue(object[] ps)
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is int))
        {
            return;
        }
        EProfileCategory category = (EProfileCategory)ps[0];
        int id = (int)ps[1];

        typePanel.AddProfileCategoryPrefab(category);
        infoPanel.ChangeValue(category, id);
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

        EventManager.TriggerEvent(EProfileEvent.ClickCharacterTab);
    }

    private void OnClickIncidentPanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfileCategoryType.Info) == false)
            typePanel.ShowScenePanel();

        EventManager.TriggerEvent(EProfileEvent.ClickIncidentTab);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoInProfile, ChangeValue);
    }

}
