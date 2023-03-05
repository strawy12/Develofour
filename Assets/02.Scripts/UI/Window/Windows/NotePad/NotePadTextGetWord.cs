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
            int wordIndex = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro, Input.mousePosition, Camera.main, false);

            if (wordIndex > -1)
            {
                bool isSpace = false;
                string str = "";
                TMP_CharacterInfo getCharIndexInfo = textMeshPro.textInfo.characterInfo[wordIndex];
                
                char c = getCharIndexInfo.character;

                int count = wordIndex;

                while(!isSpace)
                {
                    count--;
                    if(count == -1)
                    {
                        isSpace = true;
                        break;
                    }
                    getCharIndexInfo = textMeshPro.textInfo.characterInfo[count];
                    c = getCharIndexInfo.character;
                    if(c == ' ')
                    {
                        isSpace = true;
                        break;
                    }
                    str = c + str;
                }

                isSpace = false;
                count = wordIndex;

                while (!isSpace)
                {
                    count++;
                    if (count > textMeshPro.textInfo.characterCount - 1 )
                    {
                        isSpace = true;
                        break;
                    }
                    getCharIndexInfo = textMeshPro.textInfo.characterInfo[count - 1];
                    c = getCharIndexInfo.character;
                    if (c == ' ')
                    {
                        isSpace = true;
                        break;
                    }
                    str = str + c;
                }
                Debug.Log(str);
                GetProfilerWordSystem.OnGeneratedProfiler.Invoke(str);
            }
        }
    }
}
