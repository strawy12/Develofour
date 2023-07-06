using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProfilerPanel : MonoBehaviour
{
    public ProfilerInfoPanel infoPanel;

    [SerializeField]
    private ProfilerInventoryPanel typePanel;
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
        EventManager.StartListening(EProfilerEvent.RegisterInfo, ChangeValue);

        Show();

    }
    public void ChangeValue(object[] ps)
    {
        if (!(ps[0] is EProfilerCategory) || !(ps[1] is int))
        {
            return;
        }
        EProfilerCategory category = (EProfilerCategory)ps[0];
        int id = (int)ps[1];

        typePanel.AddProfileCategoryPrefab(category);
        infoPanel.ChangeValue(category, id);
        StartCoroutine(SizeFilterCoroutine());
    }

    private IEnumerator SizeFilterCoroutine()
    {
        yield return new WaitForSeconds(0.02f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
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
        if (typePanel.CheckCurrentType(EProfilerCategoryType.Character) == false)
            typePanel.ShowCharacterPanel();

        EventManager.TriggerEvent(EProfilerEvent.ClickCharacterTab);
    }

    private void OnClickIncidentPanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfilerCategoryType.Info) == false)
            typePanel.ShowScenePanel();

        EventManager.TriggerEvent(EProfilerEvent.ClickIncidentTab);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.RegisterInfo, ChangeValue);
    }

}
