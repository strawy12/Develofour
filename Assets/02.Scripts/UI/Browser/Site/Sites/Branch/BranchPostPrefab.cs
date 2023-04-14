using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPostPrefab : MonoBehaviour
{
    public BranchPostDataSO postData;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
