using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfileInventoryPanel : MonoBehaviour
{
    private EProfileCategoryType categoryType;

    private List<ProfileCategoryDataSO> sceneCategoryList;
    private List<ProfileCategoryDataSO> characterCategoryList;

    [Header("Pool")]
    [SerializeField]
    private ProfileCategoryPrefab categoryPrefab;
    [SerializeField]
    private ProfileInventoryElements categoryParent;
    private Queue<ProfileCategoryPrefab> categorysQueue;

    private List<ProfileCategoryPrefab> categoryList;
    private int currentIndex = 0;
    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfileCategoryPrefab infoText = Instantiate(categoryPrefab, categoryParent.transform);
            infoText.Init(UnSelectAllPanel);
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
        categorysQueue = new Queue<ProfileCategoryPrefab>();
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
        InputManager.Inst.AddKeyInput(KeyCode.RightArrow, RightMove);
        InputManager.Inst.AddKeyInput(KeyCode.LeftArrow, LeftMove);

        if (categoryType == EProfileCategoryType.Scene)
        {
            ShowScenePanel();
        }
        else if (categoryType == EProfileCategoryType.Character)
        {
            ShowCharacterPanel();
        }
    }
    public void Hide()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.RightArrow, RightMove);
        InputManager.Inst.RemoveKeyInput(KeyCode.LeftArrow, LeftMove);
        gameObject.SetActive(false);
    }
    public void AddProfileCategoryPrefab(EProfileCategory category)
    {
        if (categoryType == EProfileCategoryType.Character)
        {
            foreach (var data in characterCategoryList)
            {
                if (data.category == category)
                {
                    var prefab = categoryList.Find(x => x.CurrentData.category == category);
                    prefab.Show(data);
                }
            }
        }
    }
    public void RightMove()
    {

        if (categoryList.Count == 1 || currentIndex <= 0)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex -= 1;
        }

        categoryList[currentIndex].Select();
    }
    public void LeftMove()
    {
        if (currentIndex >= categoryList.Count - 1)
        {
            currentIndex = categoryList.Count - 1;
        }
        else
        {
            currentIndex += 1;
        }

        categoryList[currentIndex].Select();
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

    private void UnSelectAllPanel()
    {
        var categoryObj = categoryList.Find(x => x.isSelected == true);

        currentIndex = categoryList.IndexOf(categoryObj);

        foreach (var prefab in categoryList)
        {
            prefab.UnSelect();
        }
    }
}
