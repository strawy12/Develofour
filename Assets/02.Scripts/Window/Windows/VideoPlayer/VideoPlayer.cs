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

    public ImageEnlargement mainImage;

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
        //ũ�� ������ �˾Ƽ�~
        mainImage.ChangeImage(data.sprite);
        //mainImage.SetImageSizeReset(new Vector2(750f, 400f));

        startButton.button.onClick.AddListener(ButtonClick);
        //�ƾ��� ���� �������Ѽ� start���ְ�
        //���ⶩ �׳� ����
    }


    public void ButtonClick()
    {
        StartCutScene();
    }

    public void StartCutScene()
    {
        //esc ������ stop
        instanceCutScene = Instantiate(cutScene, GameManager.Inst.CutSceneCanvas.transform);
        instanceCutScene.ShowCutScene();
    }

}
