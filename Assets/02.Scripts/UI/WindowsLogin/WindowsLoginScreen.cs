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
    [SerializeField]
    private float numberWrongDuration = 3f;

    private bool isFirst = true;
    private bool isWrong;
    private void Start()
    {
        Init();

        GameManager.Inst.ChangeComputerLoginState(EComputerLoginState.Logout);

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

        passwordField.InputField.contentType = TMP_InputField.ContentType.Password;
        passwordField.InputField.characterLimit = 4;

        loginFailConfirmBtn.onClick?.AddListener(OpenLoginInputUI);

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
        if (!int.TryParse(text, out _))
        {
            if (hintText.gameObject.activeSelf == false)
            {
                hintText.gameObject.SetActive(true);
            }
            StopAllCoroutines();
            
            StartCoroutine(InputOnlyNumberCoroutine());
            passwordField.InputField.text = "";
        }
    }

    private IEnumerator InputOnlyNumberCoroutine()
    {
        hintText.text = "숫자만 입력 가능합니다";

        yield return new WaitForSeconds(numberWrongDuration);

        if(isWrong)
        {
            hintText.text = "만우절 + 새해 =  ?";
        }
        else
        {
            hintText.text = "";
        }

    }

    private void SuccessLogin()
    {
        StartCoroutine(LoadingCoroutine(() =>
        {
            GameManager.Inst.ChangeComputerLoginState(EComputerLoginState.Admin);

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
            isWrong = true;
            hintText.text = "만우절 + 새해 =  ?";

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
        {
            GuideManager.OnPlayGuide(EGuideTopicName.GuestLoginGuide, 1.5f);
        }

        loginFailUI.SetActive(true);
        loginInputUI.SetActive(false);
    }



    private void EndLogin()
    {
        EventManager.TriggerEvent(EGuideEventType.ClearGuideType, new object[1] { EGuideTopicName.GuestLoginGuide });
    }

    private void StartMonolog()
    {
        Sound.OnPlaySound(Sound.EAudioType.USBConnect);
        MonologSystem.OnEndMonologEvent += USBNoticeFunc;
        MonologSystem.OnStartMonolog(EMonologTextDataType.WindowLoginComplete, monologDelay, true);
    }

    private void USBNoticeFunc()
    {
        NoticeSystem.OnGeneratedNotice(ENoticeType.ConnectUSB, 0.5f);
        MonologSystem.OnEndMonologEvent -= USBNoticeFunc;

        GuideManager.OnPlayGuide(EGuideTopicName.LibraryOpenGuide, 40);
    }
}
