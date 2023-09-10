using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    private void SettingCallReturnData(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<CallDataSO> callDataSOList = GuidsToSOList<CallDataSO>("t:CallDataSO");
        List<CallProfileDataSO> callProfileDataSOList = GuidsToSOList<CallProfileDataSO>("t:CallProfileDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            if (columns[0] == "") continue;

            string id = columns[0].Trim();
            string[] needInfoIDList = null;

            string callScreenPath = "Assets/03.Prefabs/CallWindow/CallScreen/Return/";
            CallScreen callPrefab = null;

            callPrefab = AssetDatabase.LoadAssetAtPath<CallScreen>(callScreenPath + $"{id}.prefab");
            if (callPrefab == null)
                Debug.Log($"{id}인 Prefab이 존재하지않거나 {callScreenPath}{id}.prefab의 주소가 잘못되었습니다");



            float delay = 0f;

            if (columns.Length >= 2 && !string.IsNullOrEmpty(columns[1]))
            {
                needInfoIDList = columns[1].Split('/');
            }
            string[] AdditionalFileStrList = null;
            if (columns.Length >= 3 && !string.IsNullOrEmpty(columns[2]))
            {
                AdditionalFileStrList = columns[2].Split('/');
            }
            string returnCallID = "";
            if (columns.Length >= 4)
            {
                returnCallID = columns[3].Trim();
            }
            if (columns.Length >= 5)
            {
                float.TryParse(columns[4], out delay);
            }
            List<AdditionFile> additionFileDataList = new List<AdditionFile>();
            if (AdditionalFileStrList != null)
            {
                foreach (string str in AdditionalFileStrList)
                {
                    string[] additionalFileStr = str.Split(',');
                    AdditionFile addition = new AdditionFile() { fileID = additionalFileStr[0], directoryID = additionalFileStr[1] };
                    additionFileDataList.Add(addition);
                }
            }

            bool isCreate = false;
            CallDataSO callData = callDataSOList.Find(x => x.id == id);
            if (callData == null)
            {
                callData = CreateInstance<CallDataSO>();
                isCreate = true;
            }
            callData.id = id;
            callData.additionFileIDList = additionFileDataList;

            if (needInfoIDList != null)
            {
                callData.needInfoIDList = needInfoIDList.ToList();
            }
            if (callProfileDataSOList != null)
            {
                callData.callProfileID = callProfileDataSOList.Where(x =>
                {
                    if (x.returnCallIDList == null) return false;
                    return x.returnCallIDList.Contains(id);
                }).FirstOrDefault().id;
            }
            callData.callDataType = ECallDataType.Return;
            callData.delay = delay;
            callData.callScreen = callPrefab;
 
            string SO_PATH = $"Assets/07.ScriptableObjects/CallData/ReturnCallData/{columns[0]}.asset";

            string[] idChars = columns[0].Split('_');
            List<string> idDivision = new List<string>();

            switch (idChars[1])
            {
                case "A":
                    idDivision.Add("AssistantCall");
                    break;
                case "Y":
                    idDivision.Add("YujinCall");
                    break;
                case "P":
                    idDivision.Add("PoliceCall");
                    break;
                case "J":
                    idDivision.Add("JuYoungCall");
                    break;
                case "T":
                    idDivision.Add("TaeWoongCall");
                    break;
                case "S":
                    idDivision.Add("SecurityCall");
                    break;
                case "M":
                    idDivision.Add("Missing");
                    break;
                default:
                    idDivision.Add("EtcCall");
                    break;
            }
            SO_PATH = $"Assets/07.ScriptableObjects/CallData/OutGoingCallText/{idDivision[0]}/{id}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(callData, SO_PATH);
            }

            EditorUtility.SetDirty(callData);
            callDataSOList.Remove(callData);
        }
    }
}
