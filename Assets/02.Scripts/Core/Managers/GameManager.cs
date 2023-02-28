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

    public bool profilerTutorialClear;
    private void Start()
    {

    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        gameState = state;
        if(gameState == EGameState.CutScene)
        {
            gameStateScreenInLogin.SetActive(true);
            gameStateScreenInWindow.SetActive(true);
        }
        else
        {
            gameStateScreenInLogin.SetActive(false);
            gameStateScreenInWindow.SetActive(false);

        }
    }


}
