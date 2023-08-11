using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.TMP_InputField;

public class PasswordInputField : MonoBehaviour
{
    public SubmitEvent onSubmit => passwordField.onSubmit;
    public SelectionEvent onSelect => passwordField.onSelect;
    public SelectionEvent onDeselect => passwordField.onDeselect;
    public OnChangeEvent onValueChanged => passwordField.onValueChanged;

    public Action OnSuccessLogin;
    public Action OnFailLogin;

    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private string password;

    public TMP_InputField InputField => passwordField;

    public bool isLogin = false;

    public void Init()
    {
        passwordField.asteriskChar = '·';
        passwordField.contentType = ContentType.Password;

        passwordField.onSubmit.AddListener((a) => TryLogin());

        passwordField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);
    }

    public void SetPassword(string password)
    {
        this.password = password;
    }

    public void TryLogin()
    {
        if (string.IsNullOrEmpty(passwordField.text))
        {
            return;
        }

#if UNITY_EDITOR
        if (passwordField.text == password || passwordField.text == "11")
        {
            if (passwordField.text == "11")
            {
                Debug.LogError($"{gameObject.name} Login를 Trigger를 사용하여 클리어 했습니다. 빌드 전에 해당 Trigger를 삭제하세요");
            }
            isLogin = true;
            OnSuccessLogin?.Invoke();
        }

#else
        if (passwordField.text == password)
        {
        OnSuccessLogin?.Invoke();
        }
#endif

        else
        {
            OnFailLogin?.Invoke();

        }
    }
    public bool GetTryLoginBoolean()
    {
        if (string.IsNullOrEmpty(passwordField.text))
        {
            return false;
        }
#if UNITY_EDITOR
        if (passwordField.text == password || passwordField.text == "11")
        {
            if (passwordField.text == "11")
            {
                Debug.LogError($"{gameObject.name} Login를 Trigger를 사용하여 클리어 했습니다. 빌드 전에 해당 Trigger를 삭제하세요");
            }
            OnSuccessLogin?.Invoke();
            return true;
        }

#else
        if (passwordField.text == password)
        {
            OnSuccessLogin?.Invoke();
            return true;

        }
#endif

        else
        {
            OnFailLogin?.Invoke();
            return false;
        }
    }
}

