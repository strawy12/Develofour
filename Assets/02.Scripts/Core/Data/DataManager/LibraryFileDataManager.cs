using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddLibraryDataNewFile(string fileID)
    {
        if(saveData.libraryData.Contains(fileID))
        {
            saveData.libraryData.Remove(fileID);
        }
        saveData.libraryData.Add(fileID);
    }

}
