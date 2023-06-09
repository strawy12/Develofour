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
            //디폴트 가져와
            //근데 비디오 플레이어의 디폴트가 잇음?
        }

        cutScene = data.cutScene;

        if (cutScene == null)
        {
            Debug.LogError("CutScene 없음");
            return;
        }
        //크기 조절은 알아서~
        mainImage.ChangeImage(data.sprite);
        //mainImage.SetImageSizeReset(new Vector2(750f, 400f));

        startButton.button.onClick.AddListener(ButtonClick);
        //컷씬을 새로 생성시켜서 start해주고
        //멈출땐 그냥 삭제
    }


    public void ButtonClick()
    {
        StartCutScene();
    }

    public void StartCutScene()
    {
        //esc 누르면 stop
        instanceCutScene = Instantiate(cutScene, GameManager.Inst.CutSceneCanvas.transform);
        instanceCutScene.ShowCutScene();
    }

}
