using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage; // Assign in the inspector
    public float fadeSpeed = 1.0f; // Adjust for slower/faster fades

    private float targetAlpha = 0f;
    private Action onFadeComplete;
    private bool isFading = false;

    void Update()
    {
        if (isFading)
        {
            Color color = fadeImage.color;
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = color;

            if (Mathf.Approximately(color.a, targetAlpha))
            {
                isFading = false;
                onFadeComplete?.Invoke(); // Call the completion callback
            }
        }
    }

    public void FadeToBlack(Action onComplete = null)
    {
        StartFade(1f, onComplete);
    }

    public void FadeToClear(Action onComplete = null)
    {
        StartFade(0f, onComplete);
    }

    private void StartFade(float alpha, Action onComplete)
    {
        targetAlpha = alpha;
        onFadeComplete = onComplete;
        isFading = true;
    }
}
