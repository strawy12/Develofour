using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareSetHeight : MonoBehaviour
{
    public RectTransform my;

    public RectTransform First;
    public RectTransform Second;

    public void Compare()
    {
        if(!Second.gameObject.activeSelf)
        {
            my.sizeDelta = First.sizeDelta;
        }

        if(!First.gameObject.activeSelf)
        {
            my.sizeDelta = Second.sizeDelta;
        }

        if(First.sizeDelta.y >= Second.sizeDelta.y)
        {
            my.sizeDelta = First.sizeDelta;
        }
        else
        {
            my.sizeDelta = Second.sizeDelta;
        }
    }
}
