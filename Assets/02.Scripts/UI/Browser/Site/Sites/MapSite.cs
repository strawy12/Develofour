using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapSite : Site
{
    public Vector2 finderCoordinates;

    [SerializeField]
    private TMP_InputField longitudeInputField; // 경도
    [SerializeField]
    private TMP_InputField latitudeInputField; // 위도
    [SerializeField]
    private Button searchButton;

    [SerializeField]
    private Image mapImage;

    [SerializeField]
    private TMP_Text hintText;

    [SerializeField]
    private TMP_Text finderLocationText;


    public override void Init()
    {
        searchButton.onClick?.AddListener(SearchingCoordinates);

        longitudeInputField.onValueChanged.AddListener(CheckLongitudeInput);
        latitudeInputField.onValueChanged.AddListener(CheckLatitudeInput);
    }

    private void CheckLongitudeInput(string text)
    {
        if (!int.TryParse(text, out _))
        {
            if (longitudeInputField.text != "")
            {
                longitudeInputField.text = longitudeInputField.text.Substring(0, longitudeInputField.text.Length - 1);
            }

            StopAllCoroutines();
            StartCoroutine(InputOnlyNumberCoroutine());
        }
    }

    private void CheckLatitudeInput(string text)
    {
        if (!int.TryParse(text, out _))
        {
            if (latitudeInputField.text != "")
            {
                latitudeInputField.text = latitudeInputField.text.Substring(0, latitudeInputField.text.Length - 1);
            }

            StopAllCoroutines();
            StartCoroutine(InputOnlyNumberCoroutine());
        }
    }

    private IEnumerator InputOnlyNumberCoroutine()
    {
        hintText.text = "경도 및 위도 좌표엔 숫자만 입력 가능합니다.";

        yield return new WaitForSeconds(0.5f);
    }

    private void SearchingCoordinates()
    {
        float inputX = float.Parse(longitudeInputField.text);
        float inputY = float.Parse(latitudeInputField.text);

        if (inputX == finderCoordinates.x && inputY == finderCoordinates.y)
        {
            finderLocationText.SetText("경기도 파주시 어쩌고");
        }
    }
}

