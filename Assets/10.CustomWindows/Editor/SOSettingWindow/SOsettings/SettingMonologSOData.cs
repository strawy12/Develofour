using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingMonologSO(string dataText, string type)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:MonologTextDataSO", null);
        List<MonologTextDataSO> monologSOList = new List<MonologTextDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            monologSOList.Add(AssetDatabase.LoadAssetAtPath<MonologTextDataSO>(path));
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            if (columns[0] == string.Empty || columns[1] == string.Empty) continue;

            string id = columns[0];
            string[] textList = columns[1].Split('#');

            Color characterColor = Color.white;
            Color character2Color = Color.white;
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[4], out characterColor);
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[5], out character2Color);
            Debug.Log(characterColor);
            MonologTextDataSO monologData = monologSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (monologData == null)
            {
                monologData = CreateInstance<MonologTextDataSO>();
                isCreate = true;
            }

            List<TextData> textDataList = new List<TextData>();

            for (int j = 0; j < textList.Length; j++)
            {
                TextData data = new TextData() { text = textList[j] };

                if (textList[j].Contains("-"))
                {
                    data.textColor = characterColor;
                    data.text = data.text.Replace("-", "");
                }
                else
                {
                    data.textColor = character2Color;
                }

                textDataList.Add(data);
            }

            string[] idChars = columns[0].Split('_');
            List<string> idDivision = new List<string>();

            #region PathSetting
            switch (idChars[1].Trim())
            {
                case "CS":
                    idDivision.Add("CutSceneText");
                    break;
                case "CA":
                    idDivision.Add("CallText");
                    break;
                case "M":
                    idDivision.Add("MonologText");
                    break;
                case "G":
                    idDivision.Add("GuideText");
                    break;
                case "S":
                    idDivision.Add("StartCutSceneText");
                    break;
                default:
                    Debug.LogError($"MonologDataSO : {columns[0]}�� �з� �������� ���̵� �� : {idChars[1]}");
                    break;
            }

            if(idChars[1].Trim() == "CS")
            {
                switch (idChars[2].Trim())
                {
                    case "P":
                        idDivision.Add("PetCam");
                        break;
                    case "C":
                        idDivision.Add("CCTV");
                        break;
                    case "E":
                        idDivision.Add("Ending");
                        break;
                    default:
                        Debug.Log($"MonologDataSO : {columns[0]}�� �з� �������� ���̵� �� : {idChars[2]}");
                        idDivision.Add("EtcCutSceneTextData");
                        break;
                }
            }
            else if(idChars[1].Trim() == "CA")
            {
                switch(idChars[2].Trim())
                {
                    case "A":
                        idDivision.Add("AssistantCallText");
                        break;
                    case "Y":
                        idDivision.Add("YujinCallText");
                        break;
                    case "P":
                        idDivision.Add("PoliceCallText");
                        break;
                    case "J":
                        idDivision.Add("JuYoungCallText");
                        break;
                    case "T":
                        idDivision.Add("TaeWoongCallText");
                        break;
                    case "S":
                        idDivision.Add("SecurityCallText");
                        break;
                    case "M":
                    default:
                        idDivision.Add("EtcCallTextData");
                        break;
                }
            }

            string SO_PATH = $"Assets/07.ScriptableObjects/TextDataSO/{columns[0]}.asset";
            
            if(idDivision.Count == 1)
            {
                SO_PATH = $"Assets/07.ScriptableObjects/TextDataSO/{idDivision[0]}/{columns[0]}.asset";
            }
            else if(idDivision.Count == 2)
            {
                SO_PATH = $"Assets/07.ScriptableObjects/TextDataSO/{idDivision[0]}/{idDivision[1]}/{columns[0]}.asset";
            }
            SO_PATH = SO_PATH.Replace("\\", "/");
            #endregion

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(monologData, SO_PATH);
            }

            string path = AssetDatabase.GetAssetPath(monologData.GetInstanceID());
            string[] pathSplits = path.Split('/');
            pathSplits[pathSplits.Length - 1] = $"{columns[0]}.asset";
            string newPath = string.Join('/', pathSplits);

            if (path != newPath)
            {
                AssetDatabase.RenameAsset(path, newPath);
            }

            EditorUtility.SetDirty(monologData);
            monologSOList.Remove(monologData);
        }
        monologSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
