using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaximumBtn : Button
{
    // TODO: ���� ������ ����� Image�� ���� ����
    public TMP_Text iconImage;

    protected override void Awake()
    {
        base.Awake();
        iconImage = GetComponentInChildren<TMP_Text>();
    }
}
