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

#if UNITY_EDITOR
        CreateSaveData();
        Debug.LogWarning("PlayerData ���� �� �Ź� �ʱ�ȭ �Ǵ� ����� �ڵ尡 �����մϴ�.");
        return;
#else
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
