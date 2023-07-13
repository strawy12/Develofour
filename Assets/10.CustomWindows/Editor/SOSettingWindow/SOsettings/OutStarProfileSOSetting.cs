using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingOutStarProfileSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<OutStarProfileDataSO> profileSOList = GuidsToSOList<OutStarProfileDataSO>("t: OutStarCharacterDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string characterID = columns[0];
            List<string> timeChatList = new List<string>();

            string[] timeChats = columns[1].Trim().Split('/');

            for(int j = 0; j < timeChats.Length; j++)
            {
                if(!string.IsNullOrEmpty(timeChats[j]))
                {
                    timeChatList.Add(timeChats[j]);
                }
            }

            OutStarProfileDataSO profileData = profileSOList.Find(x => x.id == characterID);
            bool isCreate = false;

            if (profileData == null)
            {
                profileData = CreateInstance<OutStarProfileDataSO>();
                isCreate = true;
            }

            profileData.id = characterID;
            profileData.timeChatIDList = timeChatList;

            string SO_PATH = $"Assets/07.ScriptableObjects/OutStar/Profile/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(profileData, SO_PATH);
            }

            EditorUtility.SetDirty(profileData);
            profileSOList.Remove(profileData);
        }
        profileSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
