using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // 나중에 타입을 Window로 변경 할 예정
    [SerializeField]
    private GameObject windowTemp; 

   
    public Transform windowCanvasTrm;

    public void CreateWindow()
    {
    }
}
