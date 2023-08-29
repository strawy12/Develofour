using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddNewFileData(FileSO file, DirectorySO directory)
    {
        saveData.additionFileData.Add(new AdditionFileData() { fileID = file.id, directoryID = directory.id });
    }

    public AdditionFileData GetAdditionFileData(int fileID)
    {
       return saveData.additionFileData.Find(x=>x.fileID == fileID);
    }

    public bool AdditionalFileContain(FileSO file)
    {
        AdditionFileData fileData = saveData.additionFileData.Find(x => x.fileID == file.id);
        if(saveData == null)
        {
            Debug.Log("saveData is null");
        }
        else if(saveData.additionFileData == null)
        {
            Debug.Log(saveData.additionFileData);
        }
        if (fileData != null)
        {
            return true;
        }
        return false;
    }

}
