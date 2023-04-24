using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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

    private List<ProfileCategoryDataSO> sceneCategoryList;
    private List<ProfileCategoryDataSO> characterCategoryList;

    [SerializeField]
    private Sprite profilerSpeite;
    [SerializeField]
    private Button sceneBtn;
    [SerializeField]
    private Button characterBtn;

    private EProfileCategoryType categoryType;

    [Header("Pool")]
    [SerializeField]
    private ProfileCategoryPrefab categoryPrefab;
    [SerializeField]
    private Transform categoryParent;
    private Queue<ProfileCategoryPrefab> categorysQueue;

    private List<ProfileCategoryPrefab> categoryList;

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfileCategoryPrefab infoText = Instantiate(categoryPrefab, categoryParent);
            infoText.Init();
            infoText.Hide();
            categorysQueue.Enqueue(infoText);
        }
    }

    private ProfileCategoryPrefab Pop()
    {
        if (categorysQueue.Count == 0)
        {
            CreatePool();
        }

        ProfileCategoryPrefab infoText = categorysQueue.Dequeue();
        categoryList.Add(infoText);
        return infoText;
    }

    public void Push(ProfileCategoryPrefab infoText)
    {
        if (categoryList.Contains(infoText))
        {
            infoText.Hide();
            categoryList.Remove(infoText);
            categorysQueue.Enqueue(infoText);
        }
    }
    public void PushAll()
    {
        foreach (var infoText in categoryList)
        {
            categorysQueue.Enqueue(infoText);
            infoText.Hide();
        }
        categoryList.Clear();
    }
    #endregion

    public void Init()
    {
        sceneCategoryList = new List<ProfileCategoryDataSO>();
        characterCategoryList = new List<ProfileCategoryDataSO>();
        sceneCategoryList = ResourceManager.Inst.GetProfileCategoryDataList()
            .Where(x => x.Value.categoryType == EProfileCategoryType.Scene)
            .Select(x=>x.Value).ToList();
        characterCategoryList = ResourceManager.Inst.GetProfileCategoryDataList()
            .Where(x => x.Value.categoryType == EProfileCategoryType.Character)
            .Select(x => x.Value).ToList();
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
        infoPanel.Init();
        characterBtn.onClick.AddListener(OnClickCharacterPanelBtn);
        sceneBtn.onClick.AddListener(OnClickScenePanelBtn);

        categoryType = EProfileCategoryType.Scene;
        CreatePool();
    }

    private void ChangeValue(object[] ps) // string 값으로 들고옴
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];

        string key = ps[1] as string;

        if (ps[2] != null)
        {
            List<ProfileInfoTextDataSO> strList = ps[2] as List<ProfileInfoTextDataSO>;
            foreach (var temp in strList)
            {
                if (!DataManager.Inst.IsProfileInfoData(temp.category, temp.key))
                {
                    return;
                }
            }
        }

        if (category == EProfileCategory.InvisibleInformation)
        {
            return;
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        
        if(categoryType == EProfileCategoryType.Scene)
        {
            ShowScenePanel();
        }
        else if(categoryType == EProfileCategoryType.Character)
        {
            ShowCharacterPanel();
        }

    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickCharacterPanelBtn()
    {
        if (categoryType == EProfileCategoryType.Character)
        {
            return;
        }
        ShowCharacterPanel();
    }
    private void OnClickScenePanelBtn()
    {
        if (categoryType == EProfileCategoryType.Scene)
        {
            return;
        }
        ShowScenePanel();
    }

    private void ShowCharacterPanel()
    {
        categoryType = EProfileCategoryType.Character;

        foreach (var data in characterCategoryList)
        {
            ProfileCategoryPrefab categoryPrefab = Pop();
            categoryPrefab.Show(data);
        }
    }
    private void ShowScenePanel()
    {
        categoryType = EProfileCategoryType.Scene;
        foreach (var data in sceneCategoryList)
        {
            ProfileCategoryPrefab categoryPrefab = Pop();
            categoryPrefab.Show(data);
        }
    }
    public void SendAlarm(object[] ps)
    {
        if (!(ps[0] is string) || !(ps[1] is string))
        {
            return;
        }

        string key = ps[1] as string;
        string temp = "nullError";
        temp = infoPanel.SetInfoText(key);

        string text = ps[0] as string + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
        NoticeSystem.OnNotice?.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profilerSpeite, Color.white, ENoticeTag.Profiler);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, ChangeValue);
    }
}
