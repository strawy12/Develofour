using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileCategoryPrefab : MonoBehaviour
{
    private ProfileCategoryDataSO currentData;

    public void Init()
    {

    }

    public void Show(ProfileCategoryDataSO categoryData)
    {
        currentData = categoryData;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
