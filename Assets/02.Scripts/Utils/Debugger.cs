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

    public CallDataSO test;

    private void Update()
    {
        if (!isLoad) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkipScene();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 5f;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MonologSystem.OnStopMonolog?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CallSystem.OnInComingCall?.Invoke("CD_PL", "C_P_I_1");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            FileManager.Inst.AddFile("F_M_13", Constant.FileID.USB);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.Inst.WindowReset();
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
            cutScene.StopAllCoroutines();
            MonologSystem.OnStopMonolog.Invoke();
            cutScene.Skip();
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
                so.iconColor = UnityEngine.Color.black;
            }
            else if (so.windowType == EWindowType.ImageViewer)
            {
                so.iconColor = UnityEngine.Color.white;
            }
            else if (so.windowType == EWindowType.Directory)
            {
                so.iconSprite = DirectorySprite;
                so.iconColor = UnityEngine.Color.black;
            }
            else if (so.windowType == EWindowType.MediaPlayer)
            {
                so.iconSprite = mediaPlayerSprite;
                so.iconColor = UnityEngine.Color.black;
            }

            else if (so.windowType == EWindowType.ProfilerWindow)
            {
                so.iconSprite = InstallerSprite;
                so.iconColor = UnityEngine.Color.white;
            }

            else if (so.windowType == EWindowType.Browser)
            {
                so.iconColor = UnityEngine.Color.white;
            }

            else if (so.windowType == EWindowType.VideoPlayer)
            {
                so.iconColor = UnityEngine.Color.black;
            }

            else if (so.windowType == EWindowType.SoundPlayer)
            {
                so.iconColor = UnityEngine.Color.black;
            }

            else if (so.windowType == EWindowType.OutStarDM)
            {
                so.iconColor = UnityEngine.Color.white;
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(so);
        }
    }

#endif
}
