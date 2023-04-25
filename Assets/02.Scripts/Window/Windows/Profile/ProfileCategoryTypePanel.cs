using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfileCategoryTypePanel : MonoBehaviour
{
    private EProfileCategoryType categoryType;

    private List<ProfileCategoryDataSO> sceneCategoryList;
    private List<ProfileCategoryDataSO> characterCategoryList;

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
            .Select(x => x.Value).ToList();
        characterCategoryList = ResourceManager.Inst.GetProfileCategoryDataList()
            .Where(x => x.Value.categoryType == EProfileCategoryType.Character)
            .Select(x => x.Value).ToList();

        categoryType = EProfileCategoryType.Scene;
        CreatePool();
    }
    public void Show()
    {
        if (categoryType == EProfileCategoryType.Scene)
        {
            ShowScenePanel();
        }
        else if (categoryType == EProfileCategoryType.Character)
        {
            ShowCharacterPanel();
        }
    }

    public void ShowCharacterPanel()
    {
        PushAll();

        categoryType = EProfileCategoryType.Character;


        foreach (var data in characterCategoryList)
        {
            ProfileCategoryPrefab categoryPrefab = Pop();
            categoryPrefab.Show(data);
        }
    }
    public void ShowScenePanel()
    {
        PushAll();

        categoryType = EProfileCategoryType.Scene;
        foreach (var data in sceneCategoryList)
        {
            ProfileCategoryPrefab categoryPrefab = Pop();
            categoryPrefab.Show(data);
        }
    }
}
