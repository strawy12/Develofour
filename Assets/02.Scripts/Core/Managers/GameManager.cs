using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private EGameState gameState;

    public EGameState GameState { get { return gameState; } }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        object[] ps = new object[1] { typeof(NewsCutScene) };
        EventManager.TriggerEvent(EEvent.ShowCutScene, ps);
    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }
     
        gameState = state;
    }
}
