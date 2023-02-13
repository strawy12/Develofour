using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private ClickEffect clickEffect;
    [SerializeField]
    private GameObject cutScenePanel;
    private EGameState gameState;

    public EGameState GameState { get { return gameState; } }

    private void Start()
    {

    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        gameState = state;
        if(gameState == EGameState.CutScene)
        {
            cutScenePanel.SetActive(true);
        }
        else
        {
            cutScenePanel.SetActive(false);
        }
    }


}
