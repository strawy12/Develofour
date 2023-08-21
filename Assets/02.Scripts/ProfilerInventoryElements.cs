using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerInventoryElements : MonoBehaviour
{
    [SerializeField]
    private Vector2 childMaxSize = new Vector2(125f, 155f);

    private HorizontalLayoutGroup hlg;
    private Action<object[]> OnProfilerMaximum;

    public void Start()
    {
        hlg = GetComponent<HorizontalLayoutGroup>();

        OnProfilerMaximum = (a) => SetElementSize();

        EventManager.StartListening(EProfilerEvent.Maximum, OnProfilerMaximum);
    }

    public void SetElementSize()
    {
        foreach (RectTransform child in transform)
        {
            if (ProfilerWindow.CurrentProfiler.IsMaximum)
            {
                Debug.Log("Max");
                child.sizeDelta = childMaxSize;
            }
            else
            {
                Debug.Log("Min");
                child.sizeDelta = childMaxSize / 1.5f;
            }
        }

        if (ProfilerWindow.CurrentProfiler.IsMaximum)
            hlg.spacing = 30;
        else
            hlg.spacing = 20;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.Maximum, OnProfilerMaximum);
    }
}
