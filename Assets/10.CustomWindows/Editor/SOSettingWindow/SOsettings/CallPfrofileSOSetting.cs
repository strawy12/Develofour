using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    private void CallProfileSOSetting(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<CallProfileDataSO> profileDataSOList = GuidsToSOList<CallProfileDataSO>("t:CallProfileDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split("\t");

            string id = columns[0];
            string defaultTextId = columns[1];
            string notExistCallTextID = columns[2];

            string[] outgoingOptionStrList = columns[3].Split('/');

            List<CallOption> callOptionList = new List<CallOption>();

            foreach (string outgoingStr in outgoingOptionStrList)
            {
                string[] optionString = outgoingStr.Split(',');
                if (optionString.Length >= 2)
                {
                    CallOption optionData = new CallOption() { outGoingCallID = optionString[0].Trim(), decisionName = optionString[1] };
                    callOptionList.Add(optionData);
                }
            }

            string[] inComingCallStrList = columns[4].Split(',');
            string[] returnCallStrList = columns[5].Split(',');

            float delay = 0f;
            float.TryParse(columns[6], out delay);
            CallProfileDataSO profileData = profileDataSOList.Find(x => x.id == id);
            bool isCreate = false;
            if (profileData == null)
            {
                profileData = CreateInstance<CallProfileDataSO>();
                isCreate = true;
            }

            profileData.id = id;
            profileData.defaultCallID = defaultTextId;
            profileData.outGoingCallOptionList = callOptionList;
            if (inComingCallStrList[0] != "")
                profileData.inCommingCallIDList = inComingCallStrList.ToList();
            if (returnCallStrList[0] != "")
                profileData.returnCallIDList = returnCallStrList.ToList();
            profileData.delay = delay;

            string SO_PATH = $"Assets/07.ScriptableObjects/CallProfileData/CallProfile_{id}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(profileData, SO_PATH);
            }
            EditorUtility.SetDirty(profileData);
            profileDataSOList.Remove(profileData);
        }
        profileDataSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
