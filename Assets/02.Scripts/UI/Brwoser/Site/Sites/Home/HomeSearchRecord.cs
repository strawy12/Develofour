using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class HomeSearchRecord : MonoBehaviour
{
    private HomeSearchRecordDataSO recordData;
    [SerializeField]
    private HomeSearchRecordPrefab recordPrefab;
    [SerializeField]
    private int poolCnt;
    [SerializeField]
    private Transform contectTrm;

    private List<HomeSearchRecordPrefab> poolPanels = new List<HomeSearchRecordPrefab>();
    public Action OnCloseRecord;
    private bool isOpen;
    public void Init(HomeSearchRecordDataSO data)
    {
        Debug.Log("11");
        if (poolPanels.Count < 5)
        {
            CreatePool();
        }

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
            obj.gameObject.SetActive(false);
        }
    }

    private void SettingString()
    {
        for(int i = 0; i < recordData.records.Count; i++)
        {
            poolPanels[i].Init(recordData.records[i]);
        }
    }

    public void OpenPanel()
    {
        isOpen = true;
        gameObject.SetActive(true);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }
    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
        isOpen = false;
        OnCloseRecord?.Invoke();
    }
}
