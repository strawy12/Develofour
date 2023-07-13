using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingOutStarChatSO(string dataText, string typeStr)
    {
        string[] rows = dataText.Split('\n');

        List<OutStarChatDataSO> chatSOList = GuidsToSOList<OutStarChatDataSO>("t: OutStarChatDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0];
            bool isMine = bool.Parse(columns[1]);
            string chat = columns[2];

            List<OutStarTrigger> triggerList = new List<OutStarTrigger>();
            if(!string.IsNullOrEmpty(columns[3]))
            {
                string[] triggers = columns[3].Trim().Split('/');
                for (int j = 0; j < triggers.Length; j++)
                {
                    string[] values = triggers[j].Trim().Split(',');
                    int startidx = int.Parse(values[0]);
                    int endidx = int.Parse(values[1]);
                    string triggerID = values[2];
                    OutStarTrigger trigger = new OutStarTrigger(triggerID, startidx, endidx);
                    triggerList.Add(trigger);
                }
            }


            OutStarChatDataSO chatData = chatSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (chatData == null)
            {
                chatData = CreateInstance<OutStarChatDataSO>();
                isCreate = true;
            }

            chatData.id = id;
            chatData.isMine = isMine;
            chatData.chatText = chat;
            chatData.outStarTriggerList = triggerList;

            string[] idChars = columns[0].Split('_');

            string SO_PATH = $"Assets/07.ScriptableObjects/OutStar/Chat/{idChars[1]}/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(chatData, SO_PATH);
            }

            EditorUtility.SetDirty(chatData);
            chatSOList.Remove(chatData);
        }
        chatSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
