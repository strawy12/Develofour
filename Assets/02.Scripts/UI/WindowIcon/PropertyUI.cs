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
    private Text iconByte;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        closeButton.onClick?.AddListener(ClosePropertyTab);
        confirmButton.onClick?.AddListener(ClosePropertyTab);
    }

    public void CreatePropertyUI(WindowIconDataSO windowIconDataSO)
    {
        iconImage.sprite = windowIconDataSO.iconSprite;
        iconName.text = windowIconDataSO.iconName;
        iconByte.text = windowIconDataSO.iconByte;

        SetActive(true);
        //SetActive(true);
    }

    private void ClosePropertyTab()
    {
        SetActive(false);
    }
}
