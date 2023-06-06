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
    private Action<object[]> OnProfilerMinimum;

    public void Start()
    {
        hlg = GetComponent<HorizontalLayoutGroup>();

        OnProfilerMaximum = (a) => SetElementSize(true);
        OnProfilerMinimum = (a) => SetElementSize(false);

        EventManager.StartListening(EProfilerEvent.Maximum, OnProfilerMaximum);
        EventManager.StartListening(EProfilerEvent.Minimum, OnProfilerMinimum);
    }

    public void SetElementSize(bool isMaximum)
    {
        foreach (RectTransform child in transform)
        {
            if (isMaximum)
            {
                child.sizeDelta = childMaxSize;
            }
            else
            {
                child.sizeDelta = childMaxSize / 1.5f;
            }
        }

        if (isMaximum)
            hlg.spacing = 30;
        else
            hlg.spacing = 20;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.Maximum, OnProfilerMaximum);
        EventManager.StopListening(EProfilerEvent.Minimum, OnProfilerMinimum);
    }
}
