using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingCategorySO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<ProfilerCategoryDataSO> categorySOList = GuidsToSOList<ProfilerCategoryDataSO>("t: ProfilerCategoryDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            string id = columns[0];
            EProfilerCategoryType type = (EProfilerCategoryType)Enum.Parse(typeof(EProfilerCategoryType), columns[1]);
            string categoryName = columns[2];
            string defaultID = columns[3];
            string[] infoIDStrList = columns[4].Split('/');
            List<string> infoIDList = new List<string>();
            for (int j = 0; j < infoIDStrList.Length; j++)
            {
                infoIDList.Add(infoIDStrList[j].Trim());
            }

            ProfilerCategoryDataSO categoryData = categorySOList.Find(x => x.id == id);
            bool isCreate = false;

            if (categoryData == null)
            {
                categoryData = CreateInstance<ProfilerCategoryDataSO>();
                isCreate = true;
            }

            categoryData.id = id;
            categoryData.categoryType = type;
            categoryData.categoryName = categoryName;
            categoryData.defaultInfoID = defaultID;
            categoryData.infoIDList = infoIDList;

            #region PathSetting
            string SO_PATH = $"Assets/07.ScriptableObjects/ProfilerCategory/{columns[0]}.asset";

            List<string> idDivision = new List<string>();

            switch (type)
            {
                case EProfilerCategoryType.Character:
                    idDivision.Add("Character");
                    break;
                case EProfilerCategoryType.Info:
                    idDivision.Add("Info");
                    break;
                case EProfilerCategoryType.Visiable:
                    idDivision.Add("Visiable");
                    break;
                default:
                    Debug.LogError($"ProfilerCategoryData : {columns[0]}에 분류 되지않은 아이디 값");
                    break;
            }
            if (idDivision.Count == 1)
            {
                SO_PATH = $"Assets/07.ScriptableObjects/Profiler/CategoryData/{idDivision[0]}/{columns[0]}.asset";
            }

            #endregion

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(categoryData, SO_PATH);
            }

            string path = AssetDatabase.GetAssetPath(categoryData.GetInstanceID());
            string[] pathSplits = path.Split('/');
            pathSplits[pathSplits.Length - 1] = $"{columns[0]}.asset";
  

            EditorUtility.SetDirty(categoryData);
            categorySOList.Remove(categoryData);
        }
        categorySOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
