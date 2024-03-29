﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ProfilerInventoryPanel : MonoBehaviour
{
    private EProfilerCategoryType categoryType;
    [SerializeField]
    private List<ProfilerCategoryDataSO> sceneCategoryList;
    [SerializeField]
    private List<ProfilerCategoryDataSO> characterCategoryList;

    [Header("Pool")]
    [SerializeField]
    private ProfilerCategoryPrefab categoryTemp;
    [SerializeField]
    private Transform categoryParent;

    private Queue<ProfilerCategoryPrefab> categorysQueue;

    private List<ProfilerCategoryPrefab> categoryList;
    private int currentIndex = 0;

#if UNITY_EDITOR

    [ContextMenu("SetCategoryList")]
    public void SetCategoryList_Debug()
    {
        sceneCategoryList = new List<ProfilerCategoryDataSO>();
        characterCategoryList = new List<ProfilerCategoryDataSO>();
        string[] guids = AssetDatabase.FindAssets("t:ProfilerCategoryDataSO", null);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var so = AssetDatabase.LoadAssetAtPath<ProfilerCategoryDataSO>(path);

            if (so.categoryType == EProfilerCategoryType.Info)
            {
                sceneCategoryList.Add(so);
            }

            else if (so.categoryType == EProfilerCategoryType.Character)
            {
                characterCategoryList.Add(so);
            }

            else
            {
                Debug.LogError($"{so.name}의 카테고리 타입을 확인해보세요.");
            }
        }
    }

#endif

    #region Pool
    private void CreatePool(bool isEvidence)
    {
        for (int i = 0; i < 20; i++)
        {
            ProfilerCategoryPrefab infoText = Instantiate(categoryTemp, categoryParent);
            infoText.Init(UnSelectAllPanel, isEvidence);
            infoText.Hide();
            categorysQueue.Enqueue(infoText);
        }
    }

    private ProfilerCategoryPrefab Pop()
    {
        if (categorysQueue.Count == 0)
        {
            CreatePool(false);
        }

        ProfilerCategoryPrefab infoText = categorysQueue.Dequeue();
        categoryList.Add(infoText);
        return infoText;
    }

    public void Push(ProfilerCategoryPrefab infoText)
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
    public void Init(bool isEvidence)
    {
        categoryList = new List<ProfilerCategoryPrefab>();
        sceneCategoryList = new List<ProfilerCategoryDataSO>();
        characterCategoryList = new List<ProfilerCategoryDataSO>();
        categorysQueue = new Queue<ProfilerCategoryPrefab>();
        List<ProfilerCategoryDataSO> categoryDataList = ResourceManager.Inst.GetResourceList<ProfilerCategoryDataSO>();
        sceneCategoryList = categoryDataList.Where(x => x.categoryType == EProfilerCategoryType.Info).ToList();
        characterCategoryList = categoryDataList.Where(x => x.categoryType == EProfilerCategoryType.Character).ToList();

        categoryType = EProfilerCategoryType.None;

        CreatePool(isEvidence);
    }
    public void Show()
    {
        //InputManager.Inst.AddKeyInput(KeyCode.RightArrow, RightMove);
        //InputManager.Inst.AddKeyInput(KeyCode.LeftArrow, LeftMove);
        gameObject.SetActive(true);
        if (categoryType == EProfilerCategoryType.None)
            return;

        if (categoryType == EProfilerCategoryType.Info)
        {
            ShowScenePanel();
        }
        else if (categoryType == EProfilerCategoryType.Character)
        {
            ShowCharacterPanel();
        }
    }
    public void Hide()
    {
        //InputManager.Inst.RemoveKeyInput(KeyCode.RightArrow, RightMove);
        //InputManager.Inst.RemoveKeyInput(KeyCode.LeftArrow, LeftMove);

        gameObject.SetActive(false);
    }
    public void AddProfileCategoryPrefab(string categoryID)
    {
        if (categoryID.Contains("_V_"))
        {
            return;
        }

        if (categoryType == EProfilerCategoryType.Character)
        {
            Debug.Log("Character");

            foreach (var data in characterCategoryList)
            {
                if (data.ID == categoryID)
                {
                    Debug.Log("Same");

                    var prefab = categoryList.Find(x => x.CurrentData.ID == categoryID);
                    if (prefab == null)
                    {
                        prefab = Pop();
                    }
                    prefab.Show(data);
                }
            }
            ShowCharacterPanel();
        }
        else if (categoryType == EProfilerCategoryType.Info)
        {
            Debug.Log("Info");

            foreach (var data in sceneCategoryList)
            {
                if (data.ID == categoryID)
                {
                    Debug.Log("Same");

                    var prefab = categoryList.Find(x => x.CurrentData.ID == categoryID);
                    if (prefab == null)
                    {
                        prefab = Pop();
                    }
                    prefab.Show(data);
                }
            }
            ShowScenePanel();
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

    public bool CheckCurrentType(EProfilerCategoryType categoryType)
    {
        if (this.categoryType == categoryType)
        {
            return true;
        }
        return false;
    }

    public void ShowCharacterPanel()
    {
        PushAll();

        categoryType = EProfilerCategoryType.Character;
        EventManager.TriggerEvent(EProfilerEvent.HideInfoPanel);
        foreach (var data in characterCategoryList)
        {
            ProfilerCategoryPrefab categoryPrefab = Pop();
            categoryPrefab.Show(data);
        }

    }

    public void ShowScenePanel()
    {
        PushAll();
        EventManager.TriggerEvent(EProfilerEvent.HideInfoPanel);
        categoryType = EProfilerCategoryType.Info;

        foreach (var data in sceneCategoryList)
        {
            ProfilerCategoryPrefab categoryPrefab = Pop();
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
