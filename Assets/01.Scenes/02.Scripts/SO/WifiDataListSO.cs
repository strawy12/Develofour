using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WifiData
{
    public int wifiPower;
    public string wifiName;
}

[CreateAssetMenu(menuName ="SO/List/WifiDataList")]
public class WifiDataListSO : ScriptableObject
{
    [SerializeField]
    private List<WifiData> wifiDataList;

    public WifiData this[int idx]
    {
        get
        {
            return wifiDataList[idx];
        }
    }

    public int Count
    {
        get => wifiDataList.Count;
    }

    public void Sort()
    {
        wifiDataList.Sort();
    }
        

}
