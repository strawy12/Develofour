using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingAIChattingSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<AIChattingTextDataSO> aiChattingSOList = GuidsToSOList<AIChattingTextDataSO>("t: AIChattingTextDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0].Trim();
            string[] textList = columns[1].Trim().Split('#');

            List<AIChat> aiChatList = new List<AIChat>();
            for(int j = 0; j < textList.Length; j++)
            {
                AIChat chat = new AIChat();
                if(textList[j][0] == '%') //Image
                {
                    string[] divide = textList[j].Trim().Split('&');

                    divide[0] = divide[0].Replace("%","");
                    Debug.Log(divide[0]);
                    string spritePath = divide[0];
                    if(divide.Length == 2)
                    {
                        float sizeY = float.Parse(divide[1]);
                        chat.sizeY = sizeY;
                    }
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    if(sprite != null)
                    {
                        chat.sprite = sprite;
                    }
                }
                else
                {
                    chat.text = textList[j];
                }
                aiChatList.Add(chat);
            }

            string chatName = columns[2];

            AIChattingTextDataSO aiChattingData = aiChattingSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (aiChattingData == null)
            {
                aiChattingData = CreateInstance<AIChattingTextDataSO>();
                isCreate = true;
            }

            aiChattingData.id = id;
            aiChattingData.TextListSetting(aiChatList);

            if(!string.IsNullOrEmpty(chatName))
            {
                aiChattingData.chatName = chatName;
            }

            #region PathSetting
            string SO_PATH = $"Assets/07.ScriptableObjects/AIChattingData/{columns[0]}.asset";

            string[] idChars = columns[0].Split('_');
            List<string> idDivision = new List<string>();

            switch (idChars[1].Trim())
            {
                case "T":
                    idDivision.Add("AIChattingText");
                    break;
                case "G":
                    idDivision.Add("AIGuideText");
                    break;
                default:
                    Debug.LogError($"AIChattingSO : {columns[0]}에 분류 되지않은 아이디 값 : {idChars[1]}");
                    break;
            }
            if (idDivision.Count == 1)
            {
                SO_PATH = $"Assets/07.ScriptableObjects/AIChattingData/{idDivision[0]}/{columns[0]}.asset";
            }

            #endregion

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(aiChattingData, SO_PATH);
            }

            string path = AssetDatabase.GetAssetPath(aiChattingData.GetInstanceID());
            string[] pathSplits = path.Split('/');
            pathSplits[pathSplits.Length - 1] = $"{columns[0]}.asset";
            string newPath = string.Join('/', pathSplits);

            if (path != newPath)
            {
                AssetDatabase.RenameAsset(path, newPath);
            }

            EditorUtility.SetDirty(aiChattingData);
            aiChattingSOList.Remove(aiChattingData);

        }
        aiChattingSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
