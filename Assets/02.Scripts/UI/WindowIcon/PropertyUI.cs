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
    private TMP_Text iconName;
    [SerializeField]
    private TMP_Text iconLocation;
    [SerializeField]
    private TMP_Text iconByte;
    [SerializeField]
    private TMP_Text iconMadeData;
    [SerializeField]
    private TMP_Text iconFixData;
    [SerializeField]
    private TMP_Text iconAccessData;

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

        iconLocation.text = file.GetFileLocation();
        iconByte.text = file.GetFileBytes().ToString();
        iconMadeData.text = file.GetMadeDate();
        iconFixData.text = file.GetFixDate();
        iconAccessData.text = file.GetAccessDate();

        SetActive(true);
    }

    private void ClosePropertyTab()
    {
        SetActive(false);
    }
}
