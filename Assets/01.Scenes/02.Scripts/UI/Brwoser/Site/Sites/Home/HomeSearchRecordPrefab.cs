using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HomeSearchRecordPrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text infoText;

    public void Init(string info)
    {
        infoText.text = info;
        gameObject.SetActive(true);

    }
    public void Release()
    {
        infoText.text = "";
        gameObject.SetActive(false);
    }
}
