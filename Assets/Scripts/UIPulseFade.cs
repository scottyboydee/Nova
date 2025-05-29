using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIPulseFade : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float fadeDuration = 1f;    // Time to fade from 0 to 1 and back
    public float holdTime = 1f;        // Time to stay at full opacity
    public float minAlpha = 0f;        // Optional: make it >0 to never fully disappear

    private Image img;
    private float timer;
    private enum PulseState { FadingIn, Holding, FadingOut }
    private PulseState state = PulseState.FadingIn;

    void Awake()
    {
        img = GetComponent<Image>();
        timer = 0f;
        SetAlpha(minAlpha);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case PulseState.FadingIn:
                if (timer >= fadeDuration)
                {
                    timer = 0f;
                    state = PulseState.Holding;
                    SetAlpha(1f);
                }
                else
                {
                    float t = timer / fadeDuration;
                    SetAlpha(Mathf.Lerp(minAlpha, 1f, t));
                }
                break;

            case PulseState.Holding:
                if (timer >= holdTime)
                {
                    timer = 0f;
                    state = PulseState.FadingOut;
                }
                break;

            case PulseState.FadingOut:
                if (timer >= fadeDuration)
                {
                    timer = 0f;
                    state = PulseState.FadingIn;
                    SetAlpha(minAlpha);
                }
                else
                {
                    float t = timer / fadeDuration;
                    SetAlpha(Mathf.Lerp(1f, minAlpha, t));
                }
                break;
        }
    }

    void SetAlpha(float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }
}
