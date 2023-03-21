using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextGetWordTrigger : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerExitHandler
{
    private TMP_Text textMeshPro;

    private string word;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        word = GetWord();

        if (word != null)
        {
            GetProfilerWordSystem.OnFindWord?.Invoke(word);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.Inst.isProfilerTownloadCompleted)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (word == null)
            {
                return;
            }

            Debug.Log(word);
        }
    }

    private string GetWord()
    {
        int charIndex = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro, Input.mousePosition, Camera.main, false);

        if (charIndex > -1)
        {
            int count = charIndex;

            bool isSpace = false;
            string str = "";
            TMP_CharacterInfo getCharIndexInfo = textMeshPro.textInfo.characterInfo[charIndex];

            char c = getCharIndexInfo.character;

            while (!isSpace)
            {
                count--;
                if (count == -1)
                {
                    isSpace = true;
                    break;
                }
                getCharIndexInfo = textMeshPro.textInfo.characterInfo[count];
                c = getCharIndexInfo.character;
                if (c == ' ')
                {
                    isSpace = true;
                    break;
                }
                str = c + str;
            }

            isSpace = false;
            count = charIndex;

            while (!isSpace)
            {
                count++;
                if (count > textMeshPro.textInfo.characterCount - 1)
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

            return str;
        }
        else
        {
            return null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { "None" });
    }
}
