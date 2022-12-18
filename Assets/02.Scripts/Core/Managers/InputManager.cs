using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        keyCodes = new Dictionary<KeyCode, KeyInfo>();
    }

    void Update()
    {
        // 마우스
        for (int i = 0; i < (int)EMouseType.Cnt; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.MouseClick);

                EventManager.TriggerEvent(EInputType.InputMouseDown, new object[1] { i });
            }

            if (Input.GetMouseButton(i))
            {
                EventManager.TriggerEvent(EInputType.InputMouse, new object[1] { i });
            }

            if (Input.GetMouseButtonUp(i))
            {
                EventManager.TriggerEvent(EInputType.InputMouseUp, new object[1] { i });
            }
        }

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
}
