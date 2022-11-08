using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputCode : MonoBehaviour
{
    public Browser browser;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            browser.ChangeSite(ESiteLink.Youtube_News);
        }
    }
}