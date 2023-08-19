using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddLibraryDataNewFile(int fileid)
    {
        if (fileid == 7 || fileid == 0) return;
        if (saveData.libraryData.Contains(fileid))
        {
            saveData.libraryData.Remove(fileid);
        }
        saveData.libraryData.Add(fileid);
    }
}
