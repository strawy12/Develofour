using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    private Dictionary<EMouseType, KeyInfo> mouseEvents;
    private KeyInfo anyKeyEvent;

    private bool holdingDown;

    private void Awake()
    {
        keyCodes = new Dictionary<KeyCode, KeyInfo>();
        mouseEvents = new Dictionary<EMouseType, KeyInfo>();
        anyKeyEvent = new KeyInfo();
    }

    void Update()
    {
        #region AnyKey
        if (Input.anyKeyDown)
        {
            anyKeyEvent.OnKeyDown?.Invoke();
        }

        if (Input.anyKey)
        {
            holdingDown = true;
            anyKeyEvent.OnKeyStay?.Invoke();
        }

        if (!Input.anyKey && holdingDown)
        {
            holdingDown = false;
            anyKeyEvent.OnKeyUp?.Invoke();
        }
        #endregion

        #region Mouse
        // 마우스
        for (int i = 0; i < (int)EMouseType.Cnt; i++)
        {
            if (!mouseEvents.ContainsKey((EMouseType)i)) continue;

            if (Input.GetMouseButtonDown(i))
            {
                mouseEvents[(EMouseType)i].OnKeyDown?.Invoke();
            }

            if (Input.GetMouseButton(i))
            {
                mouseEvents[(EMouseType)i].OnKeyStay?.Invoke();
            }

            if (Input.GetMouseButtonUp(i))
            {
                mouseEvents[(EMouseType)i].OnKeyUp?.Invoke();
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

    #region Mouse
    public void AddMouseInput(EMouseType type, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (mouseEvents.ContainsKey(type) == false)
        {
            mouseEvents[type] = new KeyInfo();
        }

        KeyInfo info = mouseEvents[type];
        AddKeyEvent(info, onKeyDown, onKeyStay, onKeyUp);
    }
    public void RemoveMouseInput(EMouseType type, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (mouseEvents.ContainsKey(type) == false)
        {
            Debug.LogError($"InputManager의 KeyInfo Dictionary에 해당 키가 존재하지 않습니다. : {type}");
            return;
        }

        KeyInfo info = mouseEvents[type];
        RemoveKeyEvent(info, onKeyDown, onKeyStay, onKeyUp);
    }
    #endregion

    #region AnyKey
    public void AddAnyKeyInput(Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (anyKeyEvent == null)
        {
            anyKeyEvent = new KeyInfo();
        }

        AddKeyEvent(anyKeyEvent, onKeyDown, onKeyStay, onKeyUp);
    }
    public void RemoveAnyKeyInput(Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (anyKeyEvent == null)
        {
            Debug.LogError($"InputManager의 AnyKeyEvent가 존재하지 않습니다.");
            return;
        }

        RemoveKeyEvent(anyKeyEvent, onKeyDown, onKeyStay, onKeyUp);
    }
    #endregion

    #region Keyboard
    public void AddKeyInput(KeyCode keyCode, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (keyCodes.ContainsKey(keyCode) == false)
        {
            keyCodes[keyCode] = new KeyInfo();
        }

        KeyInfo info = keyCodes[keyCode];
        AddKeyEvent(info, onKeyDown, onKeyStay, onKeyUp);
    }
    public void RemoveKeyInput(KeyCode keyCode, Action onKeyDown = null, Action onKeyStay = null, Action onKeyUp = null)
    {
        if (keyCodes.ContainsKey(keyCode) == false)
        {
            //Debug.LogError($"InputManager의 KeyInfo Dictionary에 해당 키가 존재하지 않습니다. : {keyCode}");
            return;
        }

        KeyInfo info = keyCodes[keyCode];
        RemoveKeyEvent(info, onKeyDown, onKeyStay, onKeyUp);
    }
    #endregion

    public void AddKeyEvent(KeyInfo info, Action onKeyDown, Action onKeyStay, Action onKeyUp)
    {
        info.OnKeyDown += onKeyDown;
        info.OnKeyStay += onKeyStay;
        info.OnKeyUp += onKeyUp;
    }
    public void RemoveKeyEvent(KeyInfo info, Action onKeyDown, Action onKeyStay, Action onKeyUp)
    {
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
