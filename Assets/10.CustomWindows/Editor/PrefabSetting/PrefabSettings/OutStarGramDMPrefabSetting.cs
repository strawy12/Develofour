using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public partial class PrefabSettingWindow : EditorWindow
{
    private void OutStarGramSetting()
    {
        List<OutStarProfileDataSO> profileDataSOList = Define.GuidsToList<OutStarProfileDataSO>("t:OutStarProfileDataSO");
        Transform parentCanvasTrm = FindFirstObjectByType<Canvas>().transform;
        Debug.Log(parentCanvasTrm);
        UserChattingPanel panelTemp = AssetDatabase.LoadAssetAtPath<UserChattingPanel>("Assets/03.Prefabs/OutStarGram/ChattingPanel_Default.prefab");
        List<UserChattingPanel> chattingPanelList = new List<UserChattingPanel>();
        foreach (var profileData in profileDataSOList)
        {
            string PATH = $"Assets/03.Prefabs/OutStarGram/ChattingPanel/ChattingPanel_{profileData.id}.prefab";
            UserChattingPanel chattingPanel = AssetDatabase.LoadAssetAtPath<UserChattingPanel>(PATH);
            UserChattingPanel newChattingPanel = null;
            if (chattingPanel == null)
            {
                newChattingPanel = Instantiate(panelTemp, parentCanvasTrm);
            }
            else
            {
                newChattingPanel = Instantiate(chattingPanel, parentCanvasTrm);
            }
            newChattingPanel.gameObject.SetActive(true);
            newChattingPanel.characterID = profileData.id;
            newChattingPanel.PrefabSetting();
            newChattingPanel.name = $"ChattingPanel_{profileData.id}";
            chattingPanelList.Add(newChattingPanel);

            PrefabUtility.SaveAsPrefabAsset(newChattingPanel.gameObject, PATH);
        }
        chattingPanelList.ForEach(x => DestroyImmediate(x.gameObject));
    }
}
