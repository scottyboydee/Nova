using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPath : MonoBehaviour
{
    [SerializeField]
    private GameObject Cursor;

    [SerializeField]
    private float speed = 50;

    private float progress;

    private Vector3[] pathPoints;

    private float totalPathLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupPath();

        progress = 0;
    }

    private void SetupPath()
    {
        pathPoints = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            pathPoints[i] = child.position;

            if(i > 1)
            {
                Vector3 delta = pathPoints[i] - pathPoints[i - 1];
                totalPathLength += delta.magnitude;
            }

//            Debug.Log("Point: " + i + " pos: " + pathPoints[i] + " name: " + child.name + " totalPathLength: " + totalPathLength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( Cursor != null )
            UpdateCursor();
    }

    private void UpdateCursor()
    {
        float useSpeed = speed * Time.deltaTime;

        progress += useSpeed;

        Cursor.transform.position = GetPointFromProgress(progress);
    }

    public Vector3 GetPointFromProgress(float progress)
    {
        progress = progress % totalPathLength;

        float cumulativeDist = 0;

        Vector3 result = new Vector3();

        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            Vector3 delta = pathPoints[i + 1] - pathPoints[i];

            if (cumulativeDist + delta.magnitude < progress)
            {
                cumulativeDist += delta.magnitude;
                continue;
            }

            float remainder = progress - cumulativeDist;

            result = pathPoints[i] + ((delta * remainder) / delta.magnitude);
            break;
        }

        return result;
    }
}
