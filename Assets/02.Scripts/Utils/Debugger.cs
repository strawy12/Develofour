using System.Collections;
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

    private void Update()
    {
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


        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { EProfileCategory.InvisibleInformation, "BranchID", null });
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[2] { EMailType.BranchCertificationMail, 0.5f });
        }
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
            //if(so.windowType == EWindowType.Browser)
            //{
            //    so.color = UnityEngine.Color.white;
            //}
            //else if (so.windowType == EWindowType.SiteShortCut)
            //{
            //    so.color = UnityEngine.Color.white;
            //}
            //else if (so.windowType == EWindowType.Notepad)
            //{
            //    so.iconSprite = notepadSprite;
            //}
            //else if(so.windowType == EWindowType.ImageViewer)
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
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(so);
        }
    }

#endif

}
