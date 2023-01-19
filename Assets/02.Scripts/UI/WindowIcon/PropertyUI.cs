using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUI : MonoUI
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Text iconName;
    [SerializeField]
    private Text iconLocation;
    [SerializeField]
    private Text iconByte;
    [SerializeField]
    private Text iconMadeData;
    [SerializeField]
    private Text iconFixData;
    [SerializeField]
    private Text iconAcessData;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        closeButton.onClick?.AddListener(ClosePropertyTab);
        confirmButton.onClick?.AddListener(ClosePropertyTab);
    }

    public void CreatePropertyUI(FileSO windowIconDataSO)
    {
        iconImage.sprite = windowIconDataSO.windowIcon;
        iconName.text = windowIconDataSO.windowTitle;
        iconLocation.text = windowIconDataSO.iconLocation;
        iconByte.text = windowIconDataSO.iconByte;
        iconMadeData.text = windowIconDataSO.iconMadeData;
        iconFixData.text = windowIconDataSO.iconFixData;
        iconAcessData.text = windowIconDataSO.iconAcessData;

        SetActive(true);
        //SetActive(true);
    }

    private void ClosePropertyTab()
    {
        SetActive(false);
    }
}
