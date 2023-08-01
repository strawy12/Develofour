using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SOSettingWindow : EditorWindow
{
    public void SettingPinLockSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        List<PinLockDataSO> pinLockSOList = GuidsToSOList<PinLockDataSO>("t: PinLockDataSO");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            string id = columns[0];
            string answerPin = columns[1];
            string hint = columns[2];
            EWindowType windowType =EWindowType.None;

            bool isType = Enum.TryParse(columns[3].Trim(), out windowType);

            if(isType == false)
            {
                Debug.Log($"{columns[3].Trim()}이라는 윈도우 타입이 존재하지 않습니다.");
            }

            AutoAnswerData answerData = new AutoAnswerData();
            answerData.answer = columns[1];
            string[] decisions = columns[4].Trim().Split(',');
            for(int j = 0; j < decisions.Length; j++)
            {
                if(!string.IsNullOrEmpty(decisions[j]))
                {
                    if (answerData.needInfoData == null)
                    {
                        answerData.needInfoData = new List<string>();
                    }
                    answerData.needInfoData.Add(decisions[j]);
                }
            }

            PinLockDataSO pinLockData = pinLockSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (pinLockData == null)
            {
                pinLockData = CreateInstance<PinLockDataSO>();
                isCreate = true;
            }

            pinLockData.id = id;
            pinLockData.windowPin = answerPin;
            pinLockData.windowPinHintGuide = hint;
            pinLockData.answerData = answerData;
            pinLockData.lockWindowType = windowType;

            string SO_PATH = $"Assets/07.ScriptableObjects/PinLock/{columns[0]}_PinLock.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(pinLockData, SO_PATH);
            }

            EditorUtility.SetDirty(pinLockData);
            pinLockSOList.Remove(pinLockData);
        }
        pinLockSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
