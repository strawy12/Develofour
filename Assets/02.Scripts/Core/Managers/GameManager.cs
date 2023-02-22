using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private ClickEffect clickEffect;

    private EGameState gameState;
    
    public EGameState GameState { get { return gameState; } }
    [SerializeField]
    private GameObject gameStateScreenInLogin;
    [SerializeField]
    private GameObject gameStateScreenInWindow;
    private void Start()
    {

    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        gameState = state;
        if(gameState == EGameState.CutScene)
        {
            Debug.Log("PanelOn");
            gameStateScreenInLogin.SetActive(true);
            gameStateScreenInWindow.SetActive(true);
        }
        else
        {
            Debug.Log("PanelOff");
            gameStateScreenInLogin.SetActive(false);

            gameStateScreenInWindow.SetActive(false);

        }
    }


}
