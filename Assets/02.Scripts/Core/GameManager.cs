using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private EGameState gameState;

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }
     
        gameState = state;
    }
}
