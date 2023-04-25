using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInventoryElements : MonoBehaviour
{
    HorizontalLayoutGroup hlg;

    public void Init()
    {
        hlg = GetComponent<HorizontalLayoutGroup>();
  
    }

    public void SetElementSize(bool isMaximum)
    {
        Vector2 elementMaxSize = new Vector2(125, 155);

        foreach (RectTransform child in transform)
        {
            if (isMaximum)
            {
                child.sizeDelta = elementMaxSize;
            }
            else
            {
                child.sizeDelta = elementMaxSize / 1.5f;
            }
        }

        if (isMaximum)
            hlg.spacing = 30;
        else
            hlg.spacing = 20;
    }
}
