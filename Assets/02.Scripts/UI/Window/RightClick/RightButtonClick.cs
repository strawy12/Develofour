using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RightButtonClick : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void CreateMenu(Vector3 menuPos)
    {
        Vector3 defaultPos = new Vector3(menuPos.x + 1.35f, menuPos.y - 1.35f, menuPos.z);

        if (defaultPos.x >= 7.5f)
            defaultPos.x -= 2.4f;

        if (defaultPos.y <= -3f)
            defaultPos.y += 2.8f;

        transform.position = defaultPos;

        gameObject.SetActive(true);
    }
}
