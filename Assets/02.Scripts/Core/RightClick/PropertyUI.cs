using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUI : MonoUI
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Text iconName;
    [SerializeField]
    private Text iconByte;

    public void CreatePropertyUI(WindowIconDataSO windowIconDataSO)
    {
        iconImage.sprite = windowIconDataSO.iconSprite;
        iconName.text = windowIconDataSO.iconName;
        iconByte.text = windowIconDataSO.iconByte;

        SetActive(true);
    }
}
