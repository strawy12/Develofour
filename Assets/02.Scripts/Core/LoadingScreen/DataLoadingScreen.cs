using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using ExtenstionMethod;

public class DataLoadingScreen : MonoBehaviour
{
    [SerializeField]
    private LoadingIcon loadingIcon;
    [SerializeField]
    private TMP_Text loadingText;

    public static bool completedDataLoad { get; private set; }

    private void Awake()
    {
        StartCoroutine(Loading());
        StartCoroutine(LoadingText());

        EventManager.StartListening(ECoreEvent.EndLoadResources, (ps) => completedDataLoad = true);
    }

    private IEnumerator Loading()
    {
        float time = 0f;
        while(!completedDataLoad)
        {
            loadingIcon.StartLoading(0.75f);
            yield return new WaitForSeconds(1f);

            time += 1f;
            if(time > 60f)
            {
                // 로딩 에러
            }
        }

        EventManager.TriggerEvent(ECoreEvent.EndDataLoading);
        gameObject.SetActive(false);

    }

    private IEnumerator LoadingText()
    {
        while (!completedDataLoad)
        {
            loadingText.DOVisible(2f);
            yield return new WaitForSeconds(2f);
        }
    }
}
