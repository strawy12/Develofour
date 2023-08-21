﻿using DG.Tweening;
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
    private GameObject coverPanel;

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
    private float monologDelay = 0.3f;
    [SerializeField]
    private float numberWrongDuration = 3f;

    private bool isLoding = false;
    private bool isFirst = true;

    private void Start()
    {
        Init();

        Subscribe();
    }

    private void Init()
    {
        passwordField.Init();
        passwordField.SetPassword(password);
        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;
        passwordField.onSelect.AddListener((a) => placeHoldText.gameObject.SetActive(false));

        passwordField.InputField.contentType = TMP_InputField.ContentType.Password;
        passwordField.InputField.characterLimit = 4;


        loginFailConfirmBtn.onClick?.AddListener(OpenLoginInputUI);
        hintText.text = "만우절 + 밸런타인 데이 = XXXX";
        hintText.gameObject.SetActive(true);

        //InputManager.Inst.AddAnyKeyInput(CheckMaxInputLength);

        passwordField.InputField.onValueChanged.AddListener(CheckInputNumber);
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

    private void CheckInputNumber(string text)
    {
        if (isLoding) return;
        if (!int.TryParse(text, out _))
        {
            if (hintText.gameObject.activeSelf == false)
            {
                hintText.gameObject.SetActive(true);
            }
            if (passwordField.InputField.text != "")
            {
                passwordField.InputField.text = passwordField.InputField.text.Substring(0, passwordField.InputField.text.Length - 1);
            }

            StopAllCoroutines();
            StartCoroutine(InputOnlyNumberCoroutine());
        }
    }

    private void CheckMaxInputLength()
    {
        if (isLoding) return;

        if (passwordField.InputField.text.Length >= 4)
        {
            StartCoroutine(MaxInputFourLength());
        }
    }

    private IEnumerator MaxInputFourLength()
    {
        hintText.text = "최대 4자까지만 가능합니다";

        yield return new WaitForSeconds(numberWrongDuration);

        hintText.text = "만우절 + 밸런타인 데이 = XXXX";
    }

    private IEnumerator InputOnlyNumberCoroutine()
    {
        hintText.text = "숫자만 입력 가능합니다";

        yield return new WaitForSeconds(numberWrongDuration);

        hintText.text = "만우절 + 밸런타인 데이 = XXXX";

    }

    private void SuccessLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            Debug.Log("Login");
            EventManager.TriggerEvent(EWindowEvent.WindowsSuccessLogin);
            if (isFirst)
            {
                StartMonolog();
            }
            isFirst = false;
            EndLogin();

            windowLoginCanvas.SetActive(false);
        }));
    }

    private void FailLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            Debug.Log("Fail");
            hintText.text = "힌트: 만우절 + 밸런타인 데이";

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
        if (isLoding) yield break;

        isLoding = true;
        coverPanel.SetActive(true);
        float delay = Random.Range(0.7f, 2f);
        loadingIcon.gameObject.SetActive(true);
        while (delay > 0f)
        {
            loadingIcon.eulerAngles += (Vector3.back * loadingTurnSpeed * Time.deltaTime);
            delay -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        coverPanel.SetActive(false);
        isLoding = false;
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
        //TODO
        //if(failedLoginCnt == 1)
        //{
        //    GuideSystem.OnPlayGuide(EGuideTopicName.FirstLoginGuide, 450);
        //}
        //if (failedLoginCnt >= 5)
        //{
        //    GuideSystem.OnPlayGuide(EGuideTopicName.FirstLoginGuide, 1.5f);
        //}

        loginFailUI.SetActive(true);
        loginInputUI.SetActive(false);
    }

    private void EndLogin()
    {
        EventManager.TriggerEvent(EGuideEventType.ClearGuideType, new object[1] { EGuideTopicName.FirstLoginGuide });
        InputManager.Inst.RemoveAnyKeyInput(CheckMaxInputLength);
    }

    private void StartMonolog()
    {
        //Sound.OnPlaySound(Sound.EAudioType.USBConnect);
        int monologID = Constant.MonologKey.WINDOWS_LOGIN_COMPLETE;
        MonologSystem.OnEndMonologEvent = USBNoticeFunc;
        MonologSystem.OnStartMonolog?.Invoke(monologID, 0.1f, true);
    }

    private void USBNoticeFunc()
    {
        NoticeSystem.OnGeneratedNotice(ENoticeType.ConnectUSB, 0.5f);

        //GuideSystem.OnPlayGuide(EGuideTopicName.LibraryOpenGuide, 40);
    }
}
