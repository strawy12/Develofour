using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Debugger : MonoBehaviour
{

#if UNITY_EDITOR

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
    public Sprite backgroundBGMSprite;

    [ContextMenu("SetSOSprite")]
    public void SS()
    {
        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            FileSO so = AssetDatabase.LoadAssetAtPath<FileSO>(path);

            if (so.windowType == EWindowType.Notepad)
            {
                so.iconSprite = notepadSprite;
            }
            else if (so.windowType == EWindowType.ImageViewer)
            {
                so.iconColor = UnityEngine.Color.white;
            }
            else if (so.windowType == EWindowType.Directory)
            {
                so.iconSprite = DirectorySprite;
            }
            else if (so.windowType == EWindowType.MediaPlayer)
            {
                so.iconSprite = mediaPlayerSprite;
            }
            else if (so.windowType == EWindowType.Discord)
            {
                so.iconSprite = harmonySprite;
            }
            else if (so.windowType == EWindowType.ProfilerWindow)
            {
                so.iconSprite = InstallerSprite;
            }
            else if (so.windowType == EWindowType.BGM)
            {
                so.iconSprite = backgroundBGMSprite;
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(so);
        }
    }

#endif
}
