using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public bool IsMonologShow(string id)
    {
        return saveData.monologData.Contains(id);
    }

    public void SetMonologShow(string id)
    {
        if (!saveData.monologData.Contains(id))
        {
            saveData.monologData.Add(id);
        }
    }

}
