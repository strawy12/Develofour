using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateLoginData()
    {
        saveData.loginData = new List<bool>();

        for (int i = ((int)ELoginType.Zoogle); i < (int)ELoginType.Count; i++)
        {
            saveData.loginData.Add(false);
        }
    }
    public bool GetIsLogin(ELoginType loginType)
    {
        return saveData.loginData[(int)loginType];
    }

    public void SetIsLogin(ELoginType loginType, bool value)
    {
        saveData.loginData[(int)loginType] = value;
    }

    public void SetBranchPassword(string newPass)
    {
        saveData.branchPassword = newPass;
    }

    public bool CheckBranchPassword(string password)
    {
        if(saveData.branchPassword == "")
        {
            return false;
        }
        if (saveData.branchPassword == password)
        {
            return true;
        }
        return false;
    }
}
