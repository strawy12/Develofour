using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public void StartLoading()
    {
        gameObject.SetActive(true);
    }

    public void StopLoading()
    {
        gameObject.SetActive(false);
    }
}
