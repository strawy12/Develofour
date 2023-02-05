using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProfileSaveData
{
    public bool isShow;
    public string key;
}

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfoData")]
public class ProfileInfoDataSO : ScriptableObject
{
    [SerializeField]
    public List<ProfileSaveData> saveList;
}
