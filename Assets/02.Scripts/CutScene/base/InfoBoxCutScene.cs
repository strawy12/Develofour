using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoxCutScene : CutScene
{
    public GameObject incidentImage;
    public Image infoBoxImage;
    public TMPro.TMP_Text infoBoxText;
    [SerializeField]
    private List<Image> infoTriggerList;

    public override void ShowCutScene()
    {
        base.ShowCutScene();

        CutSceneStart_01();
    }

    private void CutSceneStart_01()
    {
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_01);
        incidentImage.SetActive(true);
    }

    private void CutSceneStart_02()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_02);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_03);
        infoTriggerList.ForEach(x=>x.color = new Color(1,1,0.312f,0.5f));
    }

    private void CutSceneStart_03()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_03);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_04);
        infoBoxImage.gameObject.SetActive(true);
    }
    private void CutSceneStart_04()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: CutSceneStart_04);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: StopCutScene);
        infoTriggerList.ForEach(x => x.color = new Color(1,0, 0f, 0.5f));
        infoBoxImage.color = new Color(1f, 0, 0, 0.5f);
        infoBoxText.text = "12 / 12"; 
    }


    public override void StopCutScene()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: StopCutScene);
        base.StopCutScene();
    }
}
