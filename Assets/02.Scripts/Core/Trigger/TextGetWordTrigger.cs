using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using ECursorState = CursorChangeSystem.ECursorState;
public class TextGetWordTrigger : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerExitHandler
{
    private TMP_Text textMeshPro;

    private string word;
    private int wordStartIndex;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        word = GetWord();

        if (word != null)
        {
            ChangeWordColor(GetProfilerWordSystem.OnFindWord.Invoke(word));
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

            GetProfilerWordSystem.OnGeneratedProfiler?.Invoke(word);
            ChangeWordColor(GetProfilerWordSystem.OnFindWord?.Invoke(word));
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
            wordStartIndex = charIndex;
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

        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
    }

    private void ChangeWordColor(ECursorState? state)
    {
        // Get the current word

        word = GetWord();

        if (word != null)
        {
            // Find the indices of the word
            int startIndex = wordStartIndex;
            int endIndex = startIndex + word.Length - 1;

            // Find the triangles that make up the word
            TMP_CharacterInfo startInfo = textMeshPro.textInfo.characterInfo[startIndex];
            TMP_CharacterInfo endInfo = textMeshPro.textInfo.characterInfo[endIndex];
            int startVertexIndex = startInfo.vertexIndex;
            int endVertexIndex = endInfo.vertexIndex + 3;
            List<int> triangles = new List<int>();
            for (int i = startVertexIndex; i < endVertexIndex; i++)
            {
                int[] triangle = textMeshPro.textInfo.meshInfo[0].triangles;
                for (int j = 0; j < triangle.Length; j += 3)
                {
                    if (triangle[j] == i || triangle[j + 1] == i || triangle[j + 2] == i)
                    {
                        triangles.Add(triangle[j]);
                        triangles.Add(triangle[j + 1]);
                        triangles.Add(triangle[j + 2]);
                    }
                }
            }

            // Change the color of the vertices
            Color32[] vertexData = textMeshPro.textInfo.meshInfo[0].colors32;


            Color color = Color.black;

            if (state == ECursorState.FindInfo) //yellow
            {
                color = Color.yellow;
            }
            else if (state == ECursorState.FoundInfo) // red
            {
                color = Color.red;
            }
            else if (state == ECursorState.Default)
            {
                color = Color.black;
            }

            foreach (int vertexIndex in triangles)
            {
                vertexData[vertexIndex] = color;
            }

            // Update the mesh
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
