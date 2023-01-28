using System.Collections;
using System.Collections.Generic;
using ExtenstionMethod;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuccessRateBar : MonoBehaviour
{
    private const int MAX_RATE = 100;
    [SerializeField] private TMP_Text successRateText;
    [SerializeField] private List<Image> successRateFieldList;

    public void SetRateBar(float rate)
    {
        rate *= MAX_RATE;
        rate = Mathf.Clamp(rate, 0, MAX_RATE);
        successRateText.text = $"{rate}%";
        SetRateField((int)rate);
    }

    private void SetRateField(int rate)
    {
        int cnt = 0;
        if (rate > 0f)
        {
            int fieldMaxRate = MAX_RATE / successRateFieldList.Count;
            while (rate >= fieldMaxRate)
            {
                successRateFieldList[cnt++].fillAmount = 1f;

                rate -= fieldMaxRate;
            }

            if (rate > 0f)
            {
                float amount = (float)rate / fieldMaxRate;
                successRateFieldList[cnt++].fillAmount = amount;

            }
        }


        for (int i = cnt; i < successRateFieldList.Count; i++)
        {
            successRateFieldList[i].fillAmount = 0f;
        }
    }
}
