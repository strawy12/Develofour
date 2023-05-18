using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ECursorState = CursorChangeSystem.ECursorState;


public class TextGetWordTrigger : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private int monoLogType;
    [SerializeField]
    private float delay;

    private TMP_Text textMeshPro;

    [SerializeField]
    private Image infoImage;

    [SerializeField]
    private List<ProfileInfoTextDataSO> needInformaitonList;

    private string word;
    private int wordStartIndex;

    private EProfileCategory category;
    private int infoID;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        word = GetWord();

        if (word != null)
        {
            ECursorState isListFinder = Define.ChangeInfoCursor(needInformaitonList, infoID);
            GetSize(isListFinder);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
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

            object[] value = GetProfilerWordSystem.OnGeneratedProfiler?.Invoke(word);

            if (value == null)
                return;

            category = (EProfileCategory)value[1];
            infoID = (int)value[0];

            if (!DataManager.Inst.IsProfileInfoData(infoID))
            {
                if (needInformaitonList.Count == 0)
                {
                    if (category != EProfileCategory.None)
                    {
                        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, infoID, null });
                    }
                    else
                    {
                        MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                    }
                 
                    GetSize(GetProfilerWordSystem.OnFindWord.Invoke(word));
                }
                else
                {
                    foreach (ProfileInfoTextDataSO needData in needInformaitonList)
                    {
                        MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                        return;
                    }

                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, infoID, null });
                    GetSize(GetProfilerWordSystem.OnFindWord.Invoke(word));
                }
            }
        }
    }

    private string GetWord()
    {
        int charIndex = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro, Input.mousePosition, Define.MainCam, false);

        if (charIndex > -1 && charIndex < textMeshPro.maxVisibleCharacters)
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
                if (c == ' ' || c == '\n')
                {
                    isSpace = true;
                    break;
                }
                str = c + str;
            }

            isSpace = false;
            wordStartIndex = count;
            count = charIndex;

            while (!isSpace)
            {
                count++;

                if (count > textMeshPro.maxVisibleCharacters)
                {
                    return null;
                }

                if (count > textMeshPro.textInfo.characterCount - 1)
                {
                    isSpace = true;
                    break;
                }
                getCharIndexInfo = textMeshPro.textInfo.characterInfo[count - 1];
                c = getCharIndexInfo.character;

                if (c == ' ' || c == '\n')
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
        ECursorState state = ECursorState.Default;
        infoImage.gameObject.SetActive(false);
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
    }

    private float GetSize(CursorChangeSystem.ECursorState state)
    {
        if (state == ECursorState.Default)
        {
            return 0;
        }

        int startIndex = wordStartIndex;
        int endIndex = startIndex + word.Length - 1;
        TMP_CharacterInfo startInfo = textMeshPro.textInfo.characterInfo[startIndex];
        TMP_CharacterInfo endInfo = textMeshPro.textInfo.characterInfo[endIndex];

        float x = endInfo.topRight.x - startInfo.bottomLeft.x + 15f;
        float y = endInfo.topRight.y - startInfo.bottomLeft.y + 10f;

        infoImage.rectTransform.sizeDelta = new Vector2(x, y);
        Vector3 pos = startInfo.topLeft + (endInfo.topRight - startInfo.topLeft) / 2;
        pos.x += 15;
        infoImage.rectTransform.localPosition = pos;

        if (state == ECursorState.FindInfo)
        {
            Color color = Color.yellow;
            color.a = 0.4f;
            infoImage.color = color;
        }
        else if (state == ECursorState.FoundInfo)
        {
            Color color = Color.red;
            color.a = 0.4f;
            infoImage.color = color;
        }

        infoImage.gameObject.SetActive(true);
        return 0;
    }


}
