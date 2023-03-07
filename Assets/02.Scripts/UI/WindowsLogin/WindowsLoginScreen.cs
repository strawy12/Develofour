using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WindowsLoginScreen : MonoBehaviour
{
    private int failedLoginCnt = 0;
    [SerializeField]
    private GameObject windowLoginCanvas;

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
    private TMP_Text loginFailText;
    [SerializeField]
    private GameObject loginInputUI;
    [SerializeField]
    private GameObject loginFailUI;
    [SerializeField]
    private Button loginFailConfirmBtn;
    [SerializeField]
    private float monologDelay = 0.8f;
    [Header("GuestScreen")]
    [SerializeField]
    private Button guestLoginButton;
    private bool isFirst = true;
    
    private void Start()
    {
        Init();

        //if (DataManager.Inst.CurrentPlayer.CurrentChapterData.isAdminWindowLogin)
        //{
        //    gameObject.SetActive(false);
        //    return;
        //}

        Subscribe();
    }

    private void Init()
    {
        passwordField.Init();
        passwordField.SetPassword(password);
        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;
        passwordField.onSelect.AddListener((a) => placeHoldText.gameObject.SetActive(false));
        passwordField.onDeselect.AddListener((a) => placeHoldText.gameObject.SetActive(true));

        passwordField.InputField.contentType = TMP_InputField.ContentType.Pin;
        passwordField.InputField.characterLimit = 6;

        loginFailConfirmBtn.onClick?.AddListener(OpenLoginInputUI);

        guestLoginButton.onClick?.AddListener(WindowGuestLogin);
    }

    private void Subscribe()
    {
        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyUp: Confirm);
    }

    private void Confirm()
    {
        if (loginFailConfirmBtn.gameObject.activeSelf)
        {
            OpenLoginInputUI();
        }
    }


    private void SuccessLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            DataManager.Inst.CurrentPlayer.CurrentChapterData.isAdminWindowLogin = true;
            EventManager.TriggerEvent(EWindowEvent.WindowsSuccessLogin);
            windowLoginCanvas.SetActive(false);
            if (isFirst)
            {
                StartMonolog();
            }
            isFirst = false;
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
        failedLoginCnt++;

        if (failedLoginCnt >= 5)
            loginFailText.SetText("»ß»ß-»ß»ß-»ß»ß-»ß»ß-»ß»ß");

        loginFailUI.SetActive(true);
        loginInputUI.SetActive(false);
    }

    private void WindowGuestLogin()
    {
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isGuestWindowLogin = true;
        EventManager.TriggerEvent(EWindowEvent.WindowsSuccessLogin);
        windowLoginCanvas.SetActive(false);
        if (isFirst)
        {
            StartMonolog();
        }
        isFirst = false;
    }

    private void StartMonolog()
    {
        MonologSystem.OnEndMonologEvent += USBNoticeFunc;
        MonologSystem.OnStartMonolog(ETextDataType.USBMonolog, monologDelay, 1);
    }
    private void USBNoticeFunc()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.ConnectUSB, 0f);
        MonologSystem.OnEndMonologEvent -= USBNoticeFunc;

        EventManager.TriggerEvent(ECoreEvent.OpenPlayGuide, new object[2] { 40f , "ProfilerDownGuide" });
    }
}
