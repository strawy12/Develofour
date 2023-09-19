using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingEvidenceData(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<EvidenceTypeSO> evidenceSOList = GuidsToSOList<EvidenceTypeSO>("t: EvidenceTypeSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0].Trim();
            int maxCnt = int.Parse(columns[1].Trim());
            string selectMonolog = columns[2].Trim();
            string wrongMonolog = columns[3].Trim();
            string wrongHintMonolog = columns[4].Trim();
            string answerInfoID = columns[5].Trim();
            EvidenceTypeSO evidenceData = evidenceSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (evidenceData == null)
            {
                evidenceData = CreateInstance<EvidenceTypeSO>();
                isCreate = true;
            }

            evidenceData.id = id;
            evidenceData.maxCount = maxCnt;
            evidenceData.selectMonolog = selectMonolog;
            evidenceData.wrongHintMonolog = wrongHintMonolog;
            evidenceData.wrongMonolog = wrongMonolog;

            string[] strs = answerInfoID.Split('/');
            List<string> list = new List<string>();
            foreach(var str in strs)
            {
                string s = str.Trim();
                list.Add(s);
            }

            evidenceData.answerInfoID = list;
            string SO_PATH = $"Assets/07.ScriptableObjects/EvidenceData/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(evidenceData, SO_PATH);
            }

            string path = AssetDatabase.GetAssetPath(evidenceData.GetInstanceID());
            string[] pathSplits = path.Split('/');
            pathSplits[pathSplits.Length - 1] = $"{columns[0]}.asset";
            string newPath = string.Join('/', pathSplits);

            if (path != newPath)
            {
                AssetDatabase.RenameAsset(path, newPath);
            }

            EditorUtility.SetDirty(evidenceData);
            evidenceSOList.Remove(evidenceData);

        }
        evidenceSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
