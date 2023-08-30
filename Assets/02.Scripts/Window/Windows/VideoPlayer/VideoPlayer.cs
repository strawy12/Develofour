using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoPlayer : Window
{
    public CutScene cutScene;
    private CutScene instanceCutScene;
    private VideoPlayerDataSO data;

    public Image mainImage;

    public VideoPlayerButton startButton;

    private bool isPlaying;

    protected override void Init()
    {
        base.Init();

        data = ResourceManager.Inst.GetVideoPlayerData(file.id);

        if(data == null)
        {
            //����Ʈ ������
            //�ٵ� ���� �÷��̾��� ����Ʈ�� ����?
        }

        cutScene = data.cutScene;

        if (cutScene == null)
        {
            Debug.LogError("CutScene ����");
            return;
        }

        mainImage.sprite = data.sprite;
        mainImage.rectTransform.sizeDelta = data.imageSize;
        if (data.imageSize == Vector2.zero)
        {
            mainImage.rectTransform.sizeDelta = new Vector2(750f, 400f);
        }
        startButton.button.onClick.AddListener(ButtonClick);

    }


    public void ButtonClick()
    {
        StartCutScene();
    }

    public void StartCutScene()
    {
        if (instanceCutScene != null) instanceCutScene.StopCutScene();

        Destroy(instanceCutScene);

        instanceCutScene = Instantiate(cutScene, GameManager.Inst.CutSceneCanvas.transform);
        instanceCutScene.ShowCutScene();
    }

}
