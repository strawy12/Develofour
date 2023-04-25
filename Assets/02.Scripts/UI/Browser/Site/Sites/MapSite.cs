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
        hintText.text = "�浵 �� ���� ��ǥ�� ���ڸ� �Է� �����մϴ�.";

        yield return new WaitForSeconds(0.5f);
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

