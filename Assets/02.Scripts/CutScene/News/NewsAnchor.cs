using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsAnchor : MonoBehaviour
{
    public enum EFaceType
    {
        Default,
        Think,
        Count
    }
    public enum EActType
    {
        Default,
        PointGesture,
    }

    [SerializeField]
    private float speakDelay = 0.2f;

    [SerializeField]
    private Vector2 speakOffset;

    [SerializeField]
    private List<Sprite> actSpriteList;

    [SerializeField]
    private List<Sprite> faceSpriteList;
    [SerializeField]
    private List<Sprite> speakFaceSpriteList;

    [SerializeField]
    private Image faceImage;


    public RectTransform rectTransform { get; private set; }
    public CanvasGroup canvasGroup { get; private set; }
    private Image actImage;


    private EFaceType currentFace;
    private EActType currentAct;

    private bool isSpeak;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        actImage = GetComponent<Image>();

        currentFace = EFaceType.Default;
        currentAct = EActType.Default;
    }

    public void StartSpeak()
    {
        if (isSpeak) return;
        isSpeak = true;

        StartCoroutine(SpeakCoroutine());
    }

    public void EndSpeak()
    {
        if (!isSpeak) return;
        isSpeak = false;
    }

    private IEnumerator SpeakCoroutine()
    {
        while (isSpeak)
        {
            faceImage.sprite = faceSpriteList[(int)currentFace];
            faceImage.rectTransform.anchoredPosition += speakOffset;
            yield return new WaitForSeconds(speakDelay);
            faceImage.sprite = speakFaceSpriteList[(int)currentFace];
            faceImage.rectTransform.anchoredPosition -= speakOffset;
            yield return new WaitForSeconds(speakDelay);
        }

        faceImage.sprite = faceSpriteList[(int)currentFace];
    }

    public void ChangeFace(EFaceType type)
    {
        currentFace = type;
        ChangeFace((int)type);
    }

    public void ChangeFace(int idx)
    {
        Sprite sprite = faceSpriteList[idx];
        faceImage.sprite = sprite;
    }

    public void ChangeAct(EActType type)
    {
        Sprite sprite = actSpriteList[(int)type];
        actImage.sprite = sprite;
    }
}
