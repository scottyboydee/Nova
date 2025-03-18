using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddiePathFollower : MonoBehaviour
{
    [SerializeField]
    private VectorPath Path;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float Delay = 0;

    private float delayRemaining;

    private float progress;

    // Start is called before the first frame update
    void Start()
    {
        delayRemaining = Delay;
        progress = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        delayRemaining -= Time.deltaTime;
        if (delayRemaining > 0)
            return;

        UpdatePositionFromPath();
    }

    private void UpdatePositionFromPath()
    {
        float useSpeed = Speed * Time.deltaTime;
        progress += useSpeed;

        Vector3 newPos = Path.GetPointFromProgress(progress);

        transform.position = newPos;
    }
}
