using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HomeSearchRecord : MonoBehaviour
{
    [SerializeField]
    private List<Image> recordSearchImageList;
    public void OpenPanel()
    {
        gameObject.SetActive(true);

        if (!DataManager.Inst.GetIsLogin(ELoginType.Zoogle)) // 로그인 안했다면
        {
            foreach(Image recordImage in recordSearchImageList)
            {
                recordImage.gameObject.SetActive(false);    
            }
        }
        else
        {
            foreach (Image recordImage in recordSearchImageList) // 로그인했다면
            {
                recordImage.gameObject.SetActive(true);
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
