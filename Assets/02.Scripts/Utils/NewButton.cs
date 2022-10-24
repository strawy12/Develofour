using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewButton : Button
{
    public TMP_Text text;

    protected override void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
}
