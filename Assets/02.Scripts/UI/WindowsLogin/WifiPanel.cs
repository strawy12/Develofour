using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WifiPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private List<Image> wifiPowerImageList;
    [SerializeField]
    private TMP_Text wifiNameText;
    [SerializeField]
    private TMP_Text wifiAccessLogText;

    [SerializeField]
    private float highlightAlpha;
    [SerializeField]
    private float highlightDuration;

    private Image backgroundImage;

    private WifiData wifiData;
    
    public void Init(WifiData data)
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.ChangeImageAlpha(0f);
        wifiData = data;
        SetWifiPowerUI();
        SetWifiNameText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.DOKill();
        backgroundImage.DOFade(highlightAlpha, highlightDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.DOKill();
        backgroundImage.DOFade(0f, highlightDuration);
    }

    public void SetWifiNameText()
    {
        wifiNameText.SetText($"{wifiData.wifiName}\n<color=#999999>{wifiData.wifiAccessLog}전에 로그인 됨</color>");
    }

    public void SetWifiPowerUI()
    {
        for(int i = 0; i < wifiPowerImageList.Count; i++)
        {
            if(i + 1 <= wifiData.wifiPower)
            {
                wifiPowerImageList[i].ChangeImageAlpha(1f);
            }
            else
            {
                wifiPowerImageList[i].ChangeImageAlpha(0.1f);
            }

        }
    }
}
