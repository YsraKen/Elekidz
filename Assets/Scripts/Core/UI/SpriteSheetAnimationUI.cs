using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class SpriteSheetAnimationUI : MonoBehaviour
{
    public Sprite[] sprites;
    public float sampleRate = 12f;
    public bool unscaledDeltaTime = false;

    Image image;
    dynamic step;
    bool hasStarted;

    void Start()
    {
        image = GetComponent<Image>();
        hasStarted = true;
        OnEnable();
    }

#if UNITY_EDITOR

    void OnValidate()
    {
        if (!image)
            image = image = GetComponent<Image>();

        if (sprites.Length > 1)
            image.sprite = sprites[0];

        UpdateStepValue();
    }

#endif

    protected virtual void OnEnable()
    {
        if (!Application.isPlaying) return;
        if (!hasStarted) return;

        StartCoroutine(r());

        IEnumerator r()
        {
            UpdateStepValue();

            int i = 0;
            int length = sprites.Length;

            while (true)
            {
				image.sprite = sprites[i++ % length];
                yield return step;
            }
        }
    }

    protected virtual void OnDisable() => StopAllCoroutines();

    void UpdateStepValue()
    {
        step = unscaledDeltaTime ?
            new WaitForSecondsRealtime(1 / sampleRate) :
            new WaitForSeconds(1 / sampleRate);
    }
}