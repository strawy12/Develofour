using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    private Window turnOnWindow = null;

    public void TurnOnWindow(Window target)
    {
        turnOnWindow?.OnTurnOff?.Invoke();
        turnOnWindow = target;
        turnOnWindow?.OnTurnOn?.Invoke();
    }
}
