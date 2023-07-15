using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingProfilerInfoSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<ProfilerInfoDataSO> infoSOList = GuidsToSOList<ProfilerInfoDataSO>("t: ProfilerInfoDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            string id = columns[0];
            string name = columns[1];
            string info = columns[2];
            string notice = columns[3];

            ProfilerInfoDataSO infoData = infoSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (infoData == null)
            {
                infoData = CreateInstance<ProfilerInfoDataSO>();
                isCreate = true;
            }

            infoData.id = id;
            infoData.infoName = name;
            infoData.infomationText = info;
            infoData.noticeText = notice;

            string[] categoryIDArr = id.Trim().Split('_');
            string categoryID = "IC_" + categoryIDArr[1] + "_" + categoryIDArr[2];
            Debug.Log(categoryID);
            infoData.categoryID = categoryID;

            #region PathSetting
            string SO_PATH = $"Assets/07.ScriptableObjects/Profiler/Info/{columns[0]}.asset";

            string[] idChars = columns[0].Split('_');
            List<string> idDivision = new List<string>();

            switch (idChars[1].Trim())
            {
                case "V":
                    idDivision.Add("Visiable");
                    break;
                case "I":
                    idDivision.Add("Info");
                    break;
                case "C":
                    idDivision.Add("Character");
                    break;
                default:
                    Debug.LogError($"ProfilerInfoData : {columns[0]}에 분류 되지않은 아이디 값");
                    break;
            }
            if (idDivision.Count == 1)
            {
                SO_PATH = $"Assets/07.ScriptableObjects/Profiler/InfoData/{idDivision[0]}/{columns[0]}.asset";
            }

            #endregion

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(infoData, SO_PATH);
            }

            EditorUtility.SetDirty(infoData);
            infoSOList.Remove(infoData);
        }
        infoSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
