using UnityEngine;
using UnityEngine.UI;

public class AnimSprite : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // UI Image component
    [SerializeField] private float framesPerSecond = 10f; // Playback speed
    [SerializeField] private bool disableOnFinish = false;

    [SerializeField] private Sprite[] frames;    // Array of sprites


    private int currentFrame = 0;
    private float timer = 0f;

    void Update()
    {
        if (frames.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;

        float frameDuration = 1f / framesPerSecond;

        if (timer >= frameDuration)
        {
            timer -= frameDuration; // Preserve overflow to maintain accurate timing
            int oldFrame = currentFrame;
            currentFrame = (currentFrame + 1) % frames.Length;

            if( disableOnFinish && currentFrame < oldFrame )
            {
                gameObject.SetActive(false);
                return;
            }

            targetImage.sprite = frames[currentFrame];
        }
    }
}
