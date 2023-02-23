using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class NotePadTextGetWord : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, Input.mousePosition, Camera.main);

            if (wordIndex > -1)
            {
                TMP_WordInfo getWordIndexInfo = textMeshPro.textInfo.wordInfo[wordIndex];

                string getWord = getWordIndexInfo.GetWord();
                Debug.Log(getWord);

                GetProfilerWordSystem.OnGeneratedProfiler.Invoke(getWord);
            }
        }
    }
}
