using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoadingGauge : MonoBehaviour
{
    private int cnt = 0;
    private int maxCnt;
    [SerializeField] private Image gaugeTemp;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] private ParticleSystem successEffect;
    private List<Image> guageList;
    public void LoadComplete()
    {
        cnt++;
        SetUI();
    }

    private void SetUI()
    {
        for(int i = 0; i < cnt; i++)
        {
            guageList[i].color = Color.green;
        }

        float percent;
        if(cnt != 0)
            percent = ((float)cnt / maxCnt) * 100.0f;

        else
            percent = 0;

        percentText.text = $"{((int)percent).ToString()}%";

        if (percent >= 100.0f)
        {
            if (percentText.color == Color.green)
                return;

            percentText.color = Color.green;
            percentText.text = "<b>100%!!</b>";
            successEffect.Play();
        }
    }

    public void Init()
    {
        guageList = new List<Image>();
        maxCnt = ResourceManager.Inst.maxCnt;
        cnt = 0;
        for (int i = 0; i < maxCnt; i++)
        {
            Image gauge = Instantiate(gaugeTemp, gaugeTemp.transform.parent);
            gauge.gameObject.SetActive(true);
            gauge.color = Color.black;
            guageList.Add(gauge);
        }

        SetUI();
    }
}
