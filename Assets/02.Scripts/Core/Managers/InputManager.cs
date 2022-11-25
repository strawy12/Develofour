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
    private List<KeyInfo> keyCodes;

    private void Awake()
    {
        keyCodes = new List<KeyInfo>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        // 마우스
        for (int i = 0; i < (int)EMouseType.Cnt; i++) 
        {
            if(Input.GetMouseButtonDown(i)) 
            {
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
        foreach(var info in keyCodes)
        {
            if(info.isKeyDown && Input.GetKeyDown(info.keyCode))
            {
                EventManager.TriggerEvent(EInputType.InputKeyBoardDown, new object[1] { info.keyCode });
            }
            if (info.isKeyStay && Input.GetKey(info.keyCode))
            {
                EventManager.TriggerEvent(EInputType.InputKeyBoardStay, new object[1] { info.keyCode });
            }
            if (info.isKeyUp && Input.GetKeyUp(info.keyCode))
            {
                EventManager.TriggerEvent(EInputType.InputKeyBoardUp, new object[1] { info.keyCode });

            }
        }
    }

    private void AddInputKeyCode(KeyCode keyCode, bool isKeyDown = false, bool isKeyStay = false, bool isKeyUp = false)
    {
        keyCodes.Add(new KeyInfo() { isKeyDown = isKeyDown, isKeyStay = isKeyStay, isKeyUp = isKeyUp });
    }
}
