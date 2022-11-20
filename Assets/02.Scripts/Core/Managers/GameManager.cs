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
        //EventManager.TriggerEvent(EEvent.ShowCutScene, typeof(NewsCutScene));
    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }
     
        gameState = state;
    }
}
