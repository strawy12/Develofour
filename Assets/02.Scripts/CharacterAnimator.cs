using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private ECharacterType characterType;
    public ECharacterType Type => characterType;

    [SerializeField]
    private List<Sprite> eyesSprites;

    [SerializeField]
    private List<Sprite> mouthSprites;

    [SerializeField]
    private Image eyesImage;

    [SerializeField]
    private Image mouthImage;

    [SerializeField]
    private float blinkDuration = 0.2f;
    [SerializeField]
    private float talkDuration = 0.4f;

    [SerializeField]
    private float blinkDelay = 0.2f;
    [SerializeField]
    private float talkDelay = 0.4f;



    private bool isBlink = false;
    private bool isTalk = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayBlink();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayTalk();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isBlink = false;
            isTalk = false;
        }
    }
    public void PlayBlink()
    {
        if (isBlink) return;
        isBlink = true;

        StartCoroutine(BlinkCoroutine());
    }

    public void StopBlink()
    {
        if (!isBlink) return;
        isBlink = false;
    }

    private IEnumerator BlinkCoroutine()
    {
        while (isBlink)
        {
            foreach (Sprite sprite in eyesSprites)
            {
                eyesImage.sprite = sprite;

                yield return new WaitForSeconds(blinkDuration);
            }
            yield return new WaitForSeconds(blinkDelay);
        }

        eyesImage.sprite = eyesSprites[0];
    }

    public void PlayTalk()
    {
        if (isTalk) return;
        isTalk = true;

        StartCoroutine(TalkCoroutine());
    }

    public void StopTalk()
    {
        if (!isTalk) return;
        isTalk = false;
    }

    private IEnumerator TalkCoroutine()
    {
        while (isTalk)
        {
            foreach (Sprite sprite in mouthSprites)
            {
                mouthImage.sprite = sprite;

                yield return new WaitForSeconds(talkDuration);
            }
            yield return new WaitForSeconds(talkDelay);
        }

        mouthImage.sprite = mouthSprites[0];
    }
}
