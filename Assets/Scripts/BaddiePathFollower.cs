using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddiePathFollower : MonoBehaviour
{
    [SerializeField]
    private VectorPath Path;

    [SerializeField]
    private float Speed;

    private float progress;

    // Start is called before the first frame update
    void Start()
    {
        progress = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
