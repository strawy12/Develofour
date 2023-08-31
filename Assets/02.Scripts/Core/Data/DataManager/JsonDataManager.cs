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

        SaveDefaultJson();
    }

    public void SaveDefaultJson()
    {
        if (isInit == false) return;
        D_CheckDirectory();
        string d_data = JsonUtility.ToJson(defaultSaveData, true);
        File.WriteAllText(D_SAVE_PATH + D_SAVE_FILE, d_data);
    }

    private void LoadFromJson()
    {
        if (isInit == false) return;

        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string data = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            saveData = JsonUtility.FromJson<SaveData>(data);
            if (saveData.version != 1)
            {
                CreateSaveData();
            }
        }
        else
        {
            CreateSaveData();
        }


        if (File.Exists(D_SAVE_PATH + D_SAVE_FILE))
        {
            Debug.Log("asdf");
            string data = File.ReadAllText(D_SAVE_PATH + D_SAVE_FILE);
            defaultSaveData = JsonUtility.FromJson<DefaultSaveData>(data);
        }
        else
        {
            Debug.Log("asdf222");
            CreateDefaultSaveData();
        }

        return;


    }
}