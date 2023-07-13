using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingProfilerGuideSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<ProfilerGuideDataSO> guideSOList = GuidsToSOList<ProfilerGuideDataSO>("t: ProfilerGuideDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            string id = columns[0];
            string name = columns[1];
            bool info = bool.Parse(columns[2]);
            string textID = columns[3];

            ProfilerGuideDataSO guideData = guideSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (guideData == null)
            {
                guideData = CreateInstance<ProfilerGuideDataSO>();
                isCreate = true;
            }

            guideData.id = id;
            guideData.guideName = name;
            guideData.isAddTutorial = info;
            guideData.guideTextID = textID;

            string SO_PATH = $"Assets/07.ScriptableObjects/Profiler/Guide/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(guideData, SO_PATH);
            }

            EditorUtility.SetDirty(guideData);
            guideSOList.Remove(guideData);
        }
        guideSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
