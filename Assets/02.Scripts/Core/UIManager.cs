using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // ���߿� Ÿ���� Window�� ���� �� ����
    [SerializeField]
    private GameObject windowTemp; 

   
    public Transform windowCanvasTrm;

    public void CreateWindow()
    {
    }
}
