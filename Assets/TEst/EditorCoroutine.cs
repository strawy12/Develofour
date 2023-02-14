using System.Collections;


#if UNITY_EDITOR
public class EditorCoroutine
{
    public static EditorCoroutine StartCoroutine(IEnumerator _routine)
    {
        EditorCoroutine coroutine = new EditorCoroutine(_routine);
        coroutine.Start();
        return coroutine;
    }

    readonly IEnumerator routine;
    private EditorCoroutine(IEnumerator _routine) => routine = _routine;

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (mode == UnityEngine.SceneManagement.LoadSceneMode.Single)
            UnityEditor.EditorApplication.update -= Update;
    }

    private void Start()
    {
        UnityEditor.EditorApplication.update += Update;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Stop()
    {
        UnityEditor.EditorApplication.update -= Update;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!routine.MoveNext()) Stop();
    }
}
#endif