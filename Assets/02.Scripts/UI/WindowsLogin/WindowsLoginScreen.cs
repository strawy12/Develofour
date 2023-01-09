using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WindowsLoginScreen : MonoBehaviour
{
    [Header("Password")]
    [SerializeField]
    private PasswordInputField passwordField;

    [SerializeField]
    private string password;

    [SerializeField]
    private TMP_Text hintText;

    [SerializeField]
    private TMP_Text placeHoldText;

    [Header("Loading")]
    [SerializeField]
    private RectTransform loadingIcon;
    [SerializeField]
    private float loadingTurnSpeed;

    [Header("Login")]
    [SerializeField]
    private GameObject loginInputUI;
    [SerializeField]
    private GameObject loginFailUI;
    [SerializeField]
    private Button loginFailConfirmBtn;


    private void Start()
    {
        passwordField.Init();
        passwordField.SetPassword(password);
        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;
        passwordField.onSelect.AddListener((a) => placeHoldText.gameObject.SetActive(false));
        passwordField.onDeselect.AddListener((a) => placeHoldText.gameObject.SetActive(true));

        passwordField.InputField.contentType = TMP_InputField.ContentType.Pin;
        passwordField.InputField.characterLimit = 4;

        loginFailConfirmBtn.onClick.AddListener(OpenLoginInputUI);

        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyUp: Confirm);
    }

    private void Confirm()
    {
        if(loginFailConfirmBtn.gameObject.activeSelf)
        {
            OpenLoginInputUI();
        }
    }


    private void SuccessLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            EventManager.TriggerEvent(EQuestEvent.WriterWindowsLoginSuccess);
        }));
    }

    private void FailLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            if (hintText.gameObject.activeSelf == false)
            {
                hintText.gameObject.SetActive(true);
            }
            passwordField.InputField.text = "";

            OpenLoginFailUI();
        }));
    }

    private IEnumerator LoadingCoroutine(Action callBack)
    {
        float delay = Random.Range(0.7f, 2f);
        loadingIcon.gameObject.SetActive(true);
        while (delay > 0f)
        {
            loadingIcon.eulerAngles += (Vector3.back * loadingTurnSpeed * Time.deltaTime);
            delay -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        loadingIcon.gameObject.SetActive(false);
        callBack?.Invoke();
    }

    private void OpenLoginInputUI()
    {
        loginFailUI.SetActive(false);
        loginInputUI.SetActive(true);
    }
    private void OpenLoginFailUI()
    {
        loginFailUI.SetActive(true);
        loginInputUI.SetActive(false);
    }
}
