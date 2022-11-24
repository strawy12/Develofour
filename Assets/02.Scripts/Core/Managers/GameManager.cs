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

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ClickEffect effect = Instantiate(clickEffect, clickEffect.transform.parent);
            effect.Click();
        }
    }
}
