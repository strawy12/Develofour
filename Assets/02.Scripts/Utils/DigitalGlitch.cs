using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using UnityEditorInternal;

[RequireComponent(typeof(Camera))]
public class DigitalGlitch : MonoBehaviour
{
    public static Action<float> OnGlitchIn;
    public static Action<float> OnGlitchOut;

    [SerializeField]
    private Shader shader;

    [SerializeField, Range(0, 1)]
    private float intensity = 0;

    public float Intensity
    {
        get => intensity;
        set => intensity = value;
    }

    private Material material;
    private Texture2D noiseTexture;
    private RenderTexture trashFrame1;
    private RenderTexture trashFrame2;

    private bool isGlitch;

    //private void Start()
    //{
    //    OnGlitchIn += (duration) => StartEffect(duration, true);
    //    OnGlitchOut += (duration) => StartEffect(duration, false);
    //}

    void Update()
    {
        if (isGlitch == false) return;
        if (Random.value > Mathf.Lerp(0.9f, 0.5f, intensity))
        {
            SetUpResources();
            UpdateNoiseTexture();
        }
    }

    public void ImmediatelyStop()
    {
        intensity = 0f;
        isGlitch = false;
        enabled = false;
    }

    public void StartEffect(float duration, bool isGlitchIn)
    {
        this.enabled = true;
        if (isGlitchIn)
        {
            isGlitch = true;
        }

        DOTween.To(
            () => intensity,
            (value) => intensity = value,
            isGlitchIn ? 1f : 0f,
            duration
            ).OnComplete(() =>
            {
                if(isGlitchIn == false)
                {
                    isGlitch = false;
                }
                this.enabled = false;

            }).SetEase(Ease.InBounce);
    }


    void SetUpResources()
    {
        if (material != null) return;

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

        noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
        noiseTexture.hideFlags = HideFlags.DontSave;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;
        noiseTexture.filterMode = FilterMode.Point;

        trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
        trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
        trashFrame1.hideFlags = HideFlags.DontSave;
        trashFrame2.hideFlags = HideFlags.DontSave;

        UpdateNoiseTexture();
    }

    void UpdateNoiseTexture()
    {
        var color = Define.RandomColor();

        for (var y = 0; y < noiseTexture.height; y++)
        {
            for (var x = 0; x < noiseTexture.width; x++)
            {
                if (Random.value > 0.89f) color = Define.RandomColor();
                noiseTexture.SetPixel(x, y, color);
            }
        }

        noiseTexture.Apply();
    }



    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpResources();

        // Update trash frames on a constant interval.
        var fcount = Time.frameCount;
        if (fcount % 13 == 0) Graphics.Blit(source, trashFrame1);
        if (fcount % 73 == 0) Graphics.Blit(source, trashFrame2);

        material.SetFloat("_Intensity", intensity);
        material.SetTexture("_NoiseTex", noiseTexture);
        var trashFrame = Random.value > 0.5f ? trashFrame1 : trashFrame2;
        material.SetTexture("_TrashTex", trashFrame);

        Graphics.Blit(source, destination, material);
    }
}
