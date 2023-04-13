using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLoginChioce : MonoBehaviour
{
    [SerializeField]
    private GameObject adminLoginScreen;


    [SerializeField]
    private Button adminLoginButton;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        adminLoginButton.onClick?.AddListener(AdminLoginClick);
    }

    private void AdminLoginClick()
    {
        adminLoginScreen.SetActive(true);
    }
}
