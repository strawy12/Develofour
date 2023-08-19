using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddLibraryDataNewFile(int fileid)
    {
        saveData.libraryData.Add(fileid);
    }
}
