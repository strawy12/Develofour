using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddNewFileData(FileSO file, DirectorySO directory)
    {
        saveData.additionFileData.Add(new AdditionFileData() { fileID = file.ID, directoryID = directory.ID });
    }

    public AdditionFileData GetAdditionFileData(string fileID)
    {
       return saveData.additionFileData.Find(x=>x.fileID == fileID);
    }

    public bool AdditionalFileContain(FileSO file)
    {
        AdditionFileData fileData = saveData.additionFileData.Find(x => x.fileID == file.ID);

        if (fileData != null)
        {
            return true;
        }
        return false;
    }

}
