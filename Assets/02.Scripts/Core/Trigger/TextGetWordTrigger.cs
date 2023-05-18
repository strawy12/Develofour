using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ECursorState = CursorChangeSystem.ECursorState;

public class TextGetWordTrigger : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerExitHandler
{
    [System.Serializable]
    public class InfoClass
    {
        public int infoId;
        public string text;
        public int monologId;
        
        public List<int> needInformaitonList = new List<int>();
    }

    [SerializeField] 
    private List<InfoClass> infoDataList;


    [SerializeField]
    private TMP_Text textMeshPro;

    [SerializeField]
    private Image infoImage;

    private string word;
    private int wordStartIndex;

    public void OnPointerMove(PointerEventData eventData)
    {
        word = GetWord();

        if (word != null)
        {
            //있는 단어인지 확인
            foreach(var info in infoDataList)
            {
                if(info.text == word) //있다면
                {
                    //해당 정보 하이라이트
                    ECursorState isListFinder = Define.ChangeInfoCursor(info.needInformaitonList, info.infoId);
                    GetSize(isListFinder);
                }
            }
        }
        else
        {
            infoImage.gameObject.SetActive(false);
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

            foreach (var info in infoDataList)
            {
                if (info.text == word) //있다면
                {
                    if(info.needInformaitonList.Count != 0)
                    {
                        foreach (int needData in info.needInformaitonList)
                        {
                            if (!DataManager.Inst.IsProfileInfoData(needData))
                            {
                                return;
                            }
                        }
                    }

                    var data = ResourceManager.Inst.GetProfileInfoData(info.infoId);
                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { data.category, info.infoId });
                    MonologSystem.OnStartMonolog(info.monologId, 0, false);
                    ECursorState isListFinder = Define.ChangeInfoCursor(info.needInformaitonList, info.infoId);
                    GetSize(isListFinder);
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
        if (startIndex == -1) startIndex = 0;
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
