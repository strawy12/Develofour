using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public void StartLoading()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void StopLoading()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
