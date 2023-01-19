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
    private Text iconAccessData;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        closeButton.onClick?.AddListener(ClosePropertyTab);
        confirmButton.onClick?.AddListener(ClosePropertyTab);
    }


    public void CreatePropertyUI(FileSO file)
    {
        iconImage.sprite = file.iconSprite;
        iconName.text = file.name;

        iconLocation.text = file.windowIconData.iconLocation;
        iconByte.text = file.windowIconData.iconByte;
        iconMadeData.text = file.windowIconData.iconMadeData;
        iconFixData.text = file.windowIconData.iconFixData;
        iconAccessData.text = file.windowIconData.iconAcessData;

        SetActive(true);
    }

    private void ClosePropertyTab()
    {
        SetActive(false);
    }
}
