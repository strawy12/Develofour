using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapSite : Site
{
    public Vector2 finderCoordinates;

    [SerializeField]
    private TMP_InputField longitudeInputField; // �浵
    [SerializeField]
    private TMP_InputField latitudeInputField; // ����
    [SerializeField]
    private Button searchButton;

    [SerializeField]
    private Image mapImage;

    [SerializeField]
    private TMP_Text finderLocationText;

    public override void Init()
    {
        searchButton.onClick?.AddListener(SearchingCoordinates);
    }

    private void SearchingCoordinates()
    {
        float inputX = float.Parse(longitudeInputField.text);
        float inputY = float.Parse(latitudeInputField.text);

        if (inputX == finderCoordinates.x && inputY == finderCoordinates.y)
        {
            finderLocationText.SetText("��⵵ ���ֽ� ��¼��");
        }
    }
}

