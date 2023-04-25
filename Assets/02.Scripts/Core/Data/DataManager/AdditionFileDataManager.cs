using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddNewFileData(FileSO file, string location)
    {
        saveData.additionFileData.Add(new AdditionFileData() { fileName = file.fileName, fileLocation = location });
    }

    public bool AdditionalFileContain(FileSO file)
    {
        AdditionFileData fileData = saveData.additionFileData.Find(x => x.fileName == file.fileName);

        if (fileData != null)
        {
            return true;
        }
        return false;
    }

    public string GetAdditionalFileLocation(FileSO file)
    {
        foreach (AdditionFileData data in saveData.additionFileData)
        {
            if (data.fileName.Contains(file.fileName))
            {
                return data.fileLocation;
            }
        }
        return null;
    }
}
