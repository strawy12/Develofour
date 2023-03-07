using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private List<ImageViewerDataSO> imageVierwerList = new List<ImageViewerDataSO>();

    private Dictionary<string, ImageViewerDataSO> imageFileDictionary = new Dictionary<string, ImageViewerDataSO>();

    [SerializeField]
    private List<NotepadDataSO> notepadList = new List<NotepadDataSO>();

    private Dictionary<string, NotepadDataSO> notepadFileDictionary = new Dictionary<string, NotepadDataSO>();

    private void Awake()
    {
        InitDictionary();
    }
    private IEnumerator Start()
    {
        int cnt = 2;

        LoadAudioAssets(() => cnt--);
        LoadNoticeDatas(() => cnt--);

        yield return new WaitUntil(() => cnt == 0);

        EventManager.TriggerEvent(ECoreEvent.EndLoadResources);
    }

    private void InitDictionary()
    {
        foreach (ImageViewerDataSO imageData in imageVierwerList)
        {
            imageFileDictionary.Add(imageData.name, imageData);
        }

        foreach (NotepadDataSO notepadData in notepadList)
        {
            notepadFileDictionary.Add(notepadData.name, notepadData);
        }
    }

    public ImageViewerDataSO GetImageData(string windowName)
    {
        return imageFileDictionary[windowName];
    }

    public NotepadDataSO GetNotepadData(string windowName)
    {
        if (notepadFileDictionary[windowName] == null)
        {
            Debug.Log("해당 노트패드의 값이 존재하지 않음. Null 리턴 됨");
            return null;
        }
        return notepadFileDictionary[windowName];
    }

}
