using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaximumBtn : Button
{
    // TODO: 추후 아이콘 생기면 Image로 변경 예정
    public TMP_Text iconImage;

    protected override void Awake()
    {
        base.Awake();
        iconImage = GetComponentInChildren<TMP_Text>();
    }
}
