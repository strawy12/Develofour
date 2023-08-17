using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void SaveToJson()
    {
        if (isInit == false) return;
        CheckDirectory();

        string data = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, data);
    }

    private void LoadFromJson()
    {
        if (isInit == false) return;

        //CreateSaveData();
        //Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
        //return;

#if UNITY_EDITOR
//        CreateSaveData();
//        Debug.LogWarning("PlayerData 실행 시 매번 초기화 되는 디버깅 코드가 존재합니다.");
//        return;
//#else
        if (File.Exists(SAVE_PATH + SAVE_FILE))
                {
                    string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
                    saveData = JsonUtility.FromJson<SaveData>(data);
                }
                else
                {
                    CreateSaveData();
                }
#endif
    }
}
