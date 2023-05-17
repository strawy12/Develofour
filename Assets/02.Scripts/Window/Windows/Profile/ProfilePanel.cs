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
    TaeWoongProfile,
    CriminalInfomation,
    JuyoungMomProfile,
    SecurityProfile,
    AccompliceProfile,
    IncidentReport,
    LocationInformation,
    Bat,
    PetCam,
    CounselingRecord,
    BodyAutopsy,
    CCTV,
    LastCallRecord,
    BackgroundMail,
    ViolenceDiary,
    PrescriptionDiary,
    PetDeadDiary,
    Glove,
    Knife,
    YohanYujinTalk,
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
    }

    private void OnClickScenePanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfileCategoryType.Info) == false)
            typePanel.ShowScenePanel();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoInProfile, ChangeValue);
    }

}
