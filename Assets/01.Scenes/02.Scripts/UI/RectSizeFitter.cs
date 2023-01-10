using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RectSizeFitter : MonoBehaviour
{
    private RectTransform rectTransform;

    [ContextMenu("SetSizeDelta")]
    public void SetSizeDelta()
    {
        List<Vector3> childWorldPosList = new List<Vector3>();

        

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            RectTransform rt = rectTransform.GetChild(i) as RectTransform;



        }

    }

    private void Reset()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }
}
