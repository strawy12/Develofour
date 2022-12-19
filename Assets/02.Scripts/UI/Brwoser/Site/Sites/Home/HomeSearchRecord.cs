using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomeSearchRecord : MonoBehaviour
{
    private HomeSearchRecordDataSO recordData;

    [SerializeField]
    private HomeSearchRecordPrefab recordPrefab;
    [SerializeField]
    private int poolCnt;
    [SerializeField]
    private Transform contectTrm;

    private List<HomeSearchRecordPrefab> poolPanels;

    private void Awake()
    {
        CreatePool();
    }

    public void Init(HomeSearchRecordDataSO data)
    {
        HideAllPanel();
        recordData = data;
        SettingString();
    }
    private void CreatePool()
    {
        for(int  i = 0; i < poolCnt; i++)
        {
            HomeSearchRecordPrefab obj = Instantiate(recordPrefab, contectTrm);
            obj.Release();
            poolPanels.Add(obj);
        }
    }

    private void SettingString()
    {
        for(int i = 0; i < recordData.records.Count; i++)
        {
            poolPanels[i].Init(recordData.records[i]);
        }
    }

    private void HideAllPanel()
    {
        foreach(var panel in poolPanels)
        {
            panel.Release();
        }
    }
}
