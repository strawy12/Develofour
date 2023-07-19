using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingOutStarTimeChatSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<OutStarTimeChatDataSO> timeChatSOList = GuidsToSOList<OutStarTimeChatDataSO>("t: OutStarTimeChatDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0].Trim();

            string dateText = columns[1];

            int year = int.Parse(dateText.Substring(0, 4));
            int month = int.Parse(dateText.Substring(4, 2)); 
            int days = int.Parse(dateText.Substring(6, 2));
            int hour = int.Parse(dateText.Substring(9, 2));
            int minute = int.Parse(dateText.Substring(11, 2));

            DateTime date = new DateTime(year, month, days, hour, minute, 0);

            string[] chats = columns[2].Trim().Split('/');
            List<string> chatIDList = new List<string>();

            for(int j = 0; j < chats.Length; j++)
            {
                if(!string.IsNullOrEmpty(chats[j]))
                {
                    chatIDList.Add(chats[j].Trim());
                }
            }

            OutStarTimeChatDataSO timeChatData = timeChatSOList.Find(x => x.id == id.Trim());
            bool isCreate = false;

            if (timeChatData == null)
            {
                timeChatData = CreateInstance<OutStarTimeChatDataSO>();
                isCreate = true;
            }

            timeChatData.id = id.Trim();
            timeChatData.time = date;
            timeChatData.chatDataIDList = chatIDList;

            string[] idChars = columns[0].Split('_');

            string SO_PATH = $"Assets/07.ScriptableObjects/OutStar/TimeChat/{idChars[1]}/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(timeChatData, SO_PATH);
            }

            EditorUtility.SetDirty(timeChatData);
            timeChatSOList.Remove(timeChatData);
        }
        timeChatSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
