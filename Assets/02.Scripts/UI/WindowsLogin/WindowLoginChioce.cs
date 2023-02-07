using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLoginChioce : MonoBehaviour
{
    [SerializeField]
    private GameObject guestLoginScreen;
    [SerializeField]
    private GameObject adminLoginScreen;

    [SerializeField]
    private Button guestLoginButton;
    [SerializeField]
    private Button adminLoginButton;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        guestLoginButton.onClick?.AddListener(GuestLoginClick);
        adminLoginButton.onClick?.AddListener(AdminLoginClick);
    }

    private void GuestLoginClick()
    {
        adminLoginScreen.SetActive(false);
        guestLoginScreen.SetActive(true);
    }

    private void AdminLoginClick()
    {
        adminLoginScreen.SetActive(true);
        guestLoginScreen.SetActive(false);
    }
}
