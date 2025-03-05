using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieVertSine : MonoBehaviour
{
    [SerializeField]
    LeftRight Side;

    [SerializeField]
    float speed = 100;

    [SerializeField]
    float sineOffset = 0;

    [SerializeField]
    float sineScale = 0.1f;

    [SerializeField]
    float sineMagnitude = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void MoveVertically()
    {
        float useSpeed = speed * Time.deltaTime;

        transform.position += new Vector3(0, -useSpeed, 0);

    }

    private void MoveHorizontally()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveVertically();

        MoveHorizontally();
    }
}
