using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddSavePhoneNumber(string number)
    {
        if (!saveData.savePhoneNumber.Contains(number))
        {
            saveData.savePhoneNumber.Add(number);
        }
    }
    public bool IsSavePhoneNumber(string number)
    {
        return saveData.savePhoneNumber.Contains(number);
    }

}
