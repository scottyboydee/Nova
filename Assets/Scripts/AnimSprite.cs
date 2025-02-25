using UnityEngine;
using UnityEngine.UI;

public class AnimSprite : MonoBehaviour
{
    [SerializeField] private Image targetImage;              // UI Image component
    [SerializeField] private float framesPerSecond = 10f;      // Playback speed
    [SerializeField] private bool disableOnFinish = false;
    [SerializeField] private bool reverse = false;             // Toggle for reverse playback
    [SerializeField] private GameObject notifyObject;
    private INotify notifyTarget;

    [SerializeField] private Sprite[] frames;                // Array of sprites

    private int currentFrame = 0;
    private float timer = 0f;

    private void Reset()
    {
        // Set start frame based on playback direction
        currentFrame = reverse ? frames.Length - 1 : 0;
    }

    // Making this a set function for trackability
    public void SetReverse(bool reverse)
    {
        this.reverse = reverse;
    }

    private void Awake()
    {
        if (notifyObject != null)
        {
            notifyTarget = notifyObject.GetComponent<INotify>();
            if (notifyTarget == null)
            {
                Debug.LogError($"{notifyObject.name} does not implement INotify!");
            }
        }
    }

    void Update()
    {
        if (frames.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;
        float frameDuration = 1f / framesPerSecond;

        if (timer >= frameDuration)
        {
            timer -= frameDuration; // Preserve overflow for accurate timing

            if (!reverse)
            {
                currentFrame++;
                if (currentFrame >= frames.Length)
                {
                    if (disableOnFinish)
                    {
                        Reset();
                        if (notifyTarget != null)
                        {
                            notifyTarget.Notify(NotifyType.AnimFinished);
                        }
                        gameObject.SetActive(false);
                        return;
                    }
                    currentFrame = 0;
                }
            }
            else
            {
                currentFrame--;
                if (currentFrame < 0)
                {
                    if (disableOnFinish)
                    {
                        Reset();
                        if (notifyTarget != null)
                        {
                            notifyTarget.Notify(NotifyType.AnimFinished);
                        }
                        gameObject.SetActive(false);
                        return;
                    }
                    currentFrame = frames.Length - 1;
                }
            }

            targetImage.sprite = frames[currentFrame];
        }
    }
}
