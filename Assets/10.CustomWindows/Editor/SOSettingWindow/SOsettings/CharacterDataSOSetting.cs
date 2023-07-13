using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public partial class SOSettingWindow : EditorWindow
{
    private void SettingCharacterData(string dataText, string type) 
    {
        string[] rows = dataText.Split('\n');

        List<CharacterInfoDataSO> characterDataSOList = GuidsToSOList<CharacterInfoDataSO>("t:CharacterInfoDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0].Trim();
            string name = columns[1];
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(columns[2]);
            string phoneNumber = columns[3].Trim();
            
            CharacterInfoDataSO characterData = characterDataSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (characterData == null)
            {
                characterData = CreateInstance<CharacterInfoDataSO>();
                isCreate = true;
            }

            characterData.id = id;
            characterData.characterName = name;
            characterData.profileIcon = sprite;
            characterData.phoneNum = phoneNumber;
            string SO_PATH = $"Assets/07.ScriptableObjects/CharacterData/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(characterData, SO_PATH);
            }
            EditorUtility.SetDirty(characterData);
            characterDataSOList.Remove(characterData);
        }
        characterDataSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
