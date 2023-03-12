using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private ClickEffect clickEffect;

    private EGameState gameState;
    private EComputerLoginState computerLoginState;

    public EGameState GameState => gameState;
    public EComputerLoginState ComputerLoginState => computerLoginState;

    [SerializeField]
    private GameObject gameStateScreenInLogin;
    [SerializeField]
    private GameObject gameStateScreenInWindow;

    public bool isTutorial;

    public bool profilerTutorialClear;
    public bool isProfilerTownloadCompleted;

    private void Start()
    {

    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        if (state == EGameState.Tutorial) isTutorial = true;

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
