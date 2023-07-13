using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingTriggerSO(string dataText, string typeStr)
    {
        string[] rows = dataText.Split('\n');

        List<TriggerDataSO> triggerSOList = GuidsToSOList<TriggerDataSO>("t: TriggerDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0];
            string monologID = columns[1];

            List<string> infoIDList = new List<string>();
            string[] infoIDs = columns[2].Split(',');
            for(int j = 0; j < infoIDs.Length; j++)
            {
                if(!string.IsNullOrEmpty(infoIDs[j]))
                    infoIDList.Add(infoIDs[j]);
            }

            List<NeedInfoData> needInfoList = new List<NeedInfoData>();
            string[] needDatas = columns[3].Trim().Split('/');
            if(needDatas.Length > 0 && !string.IsNullOrEmpty(columns[3]))
            {
                for (int j = 0; j < needDatas.Length; j++)
                {
                    Debug.Log(needDatas[j]);
                    string[] needInfo = needDatas[j].Trim().Split(',');
                    
                    NeedInfoData needData = new NeedInfoData();
                    needData.needInfoID = needInfo[0];
                    needData.monologID = needInfo[1];
                    needData.getInfo = bool.Parse(needInfo[2]);
                    needInfoList.Add(needData);
                }
            }

            string completeID = columns[4];

            bool isFakeInfo = false;
            if (!string.IsNullOrEmpty(columns[5]))
            {
                isFakeInfo = bool.Parse(columns[5]);
            }
            
            string memo = string.Empty;
            if(columns.Length > 6)
            {
                memo = columns[6];
            }

            TriggerDataSO triggerData = triggerSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (triggerData == null)
            {
                triggerData = CreateInstance<TriggerDataSO>();
                isCreate = true;
            }

            triggerData.id = id;
            triggerData.monoLogType = monologID;
            triggerData.infoDataIDList = infoIDList;
            triggerData.needInfoList = needInfoList;
            triggerData.completeMonologType = completeID;
            triggerData.isFakeInfo = isFakeInfo;
            triggerData.MEMO = memo;

            string SO_PATH = $"Assets/07.ScriptableObjects/Trigger/{columns[0]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(triggerData, SO_PATH);
            }

            EditorUtility.SetDirty(triggerData);
            triggerSOList.Remove(triggerData);
        }
        triggerSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
