using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiPanelSpawner : MonoBehaviour
{
    [SerializeField]
    private WifiPanel wifiPanelPref;
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private WifiDataListSO wifiDataList;

    public void Start()
    {
        for(int i = 0; i < wifiDataList.Count; i++)
        {
            WifiPanel panel = Instantiate(wifiPanelPref, parent);
            panel.Init(wifiDataList[i]);
            panel.gameObject.SetActive(true);
        }
    }
}
