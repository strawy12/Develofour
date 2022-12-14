using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private ClickEffect clickEffect;

    public bool useCutScene;

    private EGameState gameState;

    public EGameState GameState { get { return gameState; } }

    private void Start()
    {
        if(useCutScene)
        {
            object[] ps = new object[1] { typeof(NewsCutScene) };
            EventManager.TriggerEvent(ECutSceneEvent.ShowCutScene, ps);
        }

    }
    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        gameState = state;
    }

}
