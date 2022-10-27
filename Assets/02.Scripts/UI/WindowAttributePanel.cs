using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowAttributePanel : MonoBehaviour
{
    private Button openBtn;
    private Button propertyBtn;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        openBtn = transform.Find("OpenBtn").GetComponent<Button>();
        propertyBtn = transform.Find("PropertyBtn").GetComponent<Button>();
    }
}
