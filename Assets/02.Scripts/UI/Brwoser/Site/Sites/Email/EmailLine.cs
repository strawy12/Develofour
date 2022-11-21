using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using TMPro;
using UnityEngine.UI;

public class EmailLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI informationText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Button highlightedButton;

    [SerializeField]
    private Button mailButton;

    private EmailPrefab mailPrefab;

    public void ChangeText(string name, string info, string time, EmailPrefab mailPrefab)
    {
        nameText.text = name;
        informationText.text = info;
        timeText.text = time;
        this.mailPrefab = mailPrefab;

        mailButton.onClick.AddListener(ShowMail);
    }

    public void ShowMail()
    {
        mailPrefab.ShowMail();
    }
}
