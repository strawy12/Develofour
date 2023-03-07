using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EMouseType
{
    LeftClick,
    RightClick,
    WheelClick,
    Cnt
}

public class InputManager : MonoSingleton<InputManager>
{
    private Dictionary<KeyCode, KeyInfo> keyCodes;

    private bool holdingDown;

    private void Awake()
    {
        keyCodes = new Dictionary<KeyCode, KeyInfo>();
    }

    void Update()
    {
        #region AnyKey
        if (Input.anyKeyDown)
        {
            EventManager.TriggerEvent(EInputType.InputAnyKeyDown);
        }

        if (Input.anyKey)
        {
            holdingDown = true;
            EventManager.TriggerEvent(EInputType.InputAnyKey);
        }

        if (!Input.anyKey && holdingDown)
        {
            holdingDown = false;
            EventManager.TriggerEvent(EInputType.InputAnyKeyUp);
        }
        #endregion

        #region Mouse
        // 마우스
        for (int i = 0; i < (int)EMouseType.Cnt; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                EventManager.TriggerEvent(EInputType.InputMouseDown, new object[1] { i });
            }

            if (Input.GetMouseButton(i))
            {
                EventManager.TriggerEvent(EInputType.InputMouse, new object[1] { i });
            }

            if (Input.GetMouseButtonUp(i))
            {
                //Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.MouseClickUp);

                EventManager.TriggerEvent(EInputType.InputMouseUp, new object[1] { i });
            }
        }

        #endregion

        #region Keyboard
        // 키보드
        foreach (var info in keyCodes)
        {
            if (info.Value.OnKeyDown != null && Input.GetKeyDown(info.Key))
            {
                info.Value.OnKeyDown?.Invoke();
            }
            if (info.Value.OnKeyStay != null && Input.GetKey(info.Key))
            {
                info.Value.OnKeyStay?.Invoke();
            }
            if (info.Value.OnKeyUp != null && Input.GetKeyUp(info.Key))
            {
                info.Value.OnKeyUp?.Invoke();
            }
        }

        #endregion
    }

    public void AddKeyInput(KeyCode keyCode, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (keyCodes.ContainsKey(keyCode) == false)
        {
            keyCodes[keyCode] = new KeyInfo();
        }

        KeyInfo info = keyCodes[keyCode];

        info.OnKeyDown += onKeyDown;
        info.OnKeyStay += onKeyStay;
        info.OnKeyUp += onKeyUp;
    }

    public void RemoveKeyInput(KeyCode keyCode, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (keyCodes.ContainsKey(keyCode) == false)
        {
            Debug.LogError($"InputManager의 KeyInfo Dictionary에 해당 키가 존재하지 않습니다. : {keyCode}");
            return;
        }

        KeyInfo info = keyCodes[keyCode];

        info.OnKeyDown -= onKeyDown;
        info.OnKeyStay -= onKeyStay;
        info.OnKeyUp -= onKeyUp;
    }

    public static bool AnyMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }
    public static bool AnyMouseButtonUp()
    {
        return Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2);
    }
}
