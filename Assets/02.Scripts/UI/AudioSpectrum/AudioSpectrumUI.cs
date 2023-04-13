using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioSpectrumUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform barTemp;
    [SerializeField]
    private int maxBarCount = 64;
    [SerializeField]
    private float frequency = 0.1f;

    [SerializeField]
    private float maxSize = 400f;

    [SerializeField]
    private float width = 5f;

    [SerializeField]
    private float weight = 0.5f;

    private RectTransform[] bars;

    public void Init()
    {
        bars = new RectTransform[maxBarCount];

        for (int i = 0; i < bars.Length; i++)//����� �����
        {
            bars[i] = Instantiate(barTemp, transform);//�������
            bars[i].anchoredPosition = new Vector2(i * width*2, 0);

            bars[i].sizeDelta = new Vector2(width, 1);//������ �ʱ�ȭ

            bars[i].gameObject.SetActive(true);
        }
    }

    public void StartSpectrum()
    {
        StartCoroutine(PlaySpectrum());
    }

    private IEnumerator PlaySpectrum()
    {
        while (true)
        {
            for (int i = 1; i <= bars.Length; i++)//ũ�� ����
            {
                float size = Random.Range(0f, maxSize);
                size /= (float)(i * weight);

                float duration = Random.Range(0.01f, frequency);
                bars[i - 1].DOSizeDelta(new Vector2(width, size), duration);
            }

            yield return new WaitForSeconds(frequency);
        }

    }
}
