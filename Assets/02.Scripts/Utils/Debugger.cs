﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class DebugEvent
{
    public KeyCode keyCode;
    public UnityEvent debugEvent;
}

public class Debugger : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField]
    private List<DebugEvent> debugEventList;

    [SerializeField]
    private FileSO todoFile;

    [SerializeField]
    private StartCutScene cutScene;
    private bool isLoad = false;
    private void Start()
    {
        GameManager.Inst.OnStartCallback += ActiveDebug;
    }

    [SerializeField]
    private FileSO testFileso;
    [SerializeField]
    private DirectorySO testDire;
    [SerializeField]
    private MonologTextDataSO testTextData;

    private void Update()
    {
        if (!isLoad) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkipScene();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MonologSystem.OnStopMonolog?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //디버그용 스킵 코드 이벤트까지 지워주기
            if (GameManager.Inst.GameState == EGameState.Tutorial)
                EventManager.TriggerEvent(EDebugSkipEvent.TutorialSkip);
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            FileManager.Inst.AddFile(152, 1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.ParkJuyoungProfile, 66 });
        }
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //FileManager.Inst.AddFile(FileManager.Inst.GetAdditionalFile(888), "");
        //}


    }

    private void ActiveDebug()
    {
        isLoad = true;
    }

    private void SkipScene()
    {
        if (GameManager.Inst.GameState == EGameState.DataLoading)
            return;

        if (GameManager.Inst.GameState == EGameState.PlayTitle)
            return;

        if (cutScene != null)
        {
            if (!cutScene.isSkip)
            {
                cutScene.isSkip = true;
                cutScene.DOKill(false);
                if (cutScene.isScreamSound)
                    Sound.OnImmediatelyStop(Sound.EAudioType.StartCutSceneScream);
                cutScene.StartLoading();
            }
        }

    }

    public Sprite notepadSprite;
    public Sprite BrowserSprite;
    public Sprite harmonySprite;
    public Sprite DirectorySprite;
    public Sprite InstallerSprite;
    public Sprite profileSprite;
    public Sprite mediaPlayerSprite;
    public Sprite siteShortcutSprite;
    public Sprite harmonyShortcutSprite;

    [ContextMenu("SetSOSprite")]
    public void SS()
    {
        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            FileSO so = AssetDatabase.LoadAssetAtPath<FileSO>(path);
            if (so.windowType == EWindowType.MediaPlayer)
            {
                Debug.Log(so.fileName);
                if (ResourceManager.Inst.GetMediaPlayerData(so.id) != null)
                {

                    Debug.Log(ResourceManager.Inst.GetMediaPlayerData(so.id).textData.Length * 4 + UnityEngine.Random.Range(30, 60));


                    //Debug.Log(ResourceManager.Inst.GetNotepadData(so.id).notepadBody.inputField.text.Length);
                }

                //so.color = UnityEngine.Color.white;
            }
            //else if (so.windowType == EWindowType.SiteShortCut)
            //{
            //    so.color = UnityEngine.Color.white;
            //}
            //else if (so.windowType == EWindowType.Notepad)
            //{
            //    so.iconSprite = notepadSprite;
            //}
            //else if (so.windowType == EWindowType.ImageViewer)
            //{
            //    so.color = UnityEngine.Color.white;
            //}
            //else if (so.windowType == EWindowType.Directory)
            //{
            //    so.iconSprite = DirectorySprite;
            //}
            //else if (so.windowType == EWindowType.MediaPlayer)
            //{
            //    so.iconSprite = mediaPlayerSprite;
            //}
            //else if (so.windowType == EWindowType.Discord)
            //{
            //    so.iconSprite = harmonySprite;
            //}
            //else if (so.windowType == EWindowType.ProfileWindow)
            //{
            //    so.iconSprite = InstallerSprite;
            //}
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(so);
        }
    }

#endif

}
