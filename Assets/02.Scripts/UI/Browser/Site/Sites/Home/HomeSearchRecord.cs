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

        if (!DataManager.Inst.GetIsLogin(ELoginType.Zoogle)) // �α��� ���ߴٸ�
        {
            foreach(Image recordImage in recordSearchImageList)
            {
                recordImage.gameObject.SetActive(false);    
            }
        }
        else
        {
            foreach (Image recordImage in recordSearchImageList) // �α����ߴٸ�
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
