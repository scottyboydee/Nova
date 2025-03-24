using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPath : MonoBehaviour
{
    [SerializeField]
    private GameObject Cursor;

    [SerializeField]
    private float CursorSpeed = 50;

    [SerializeField]
    private int loopRestartIndex = 0;

    private float cursorProgress;

    private Vector3[] pathPoints;

    private float totalPathLength = 0;

    private float loopPathLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupPath();

        cursorProgress = 0;
    }

    private void SetupPath()
    {
        pathPoints = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            pathPoints[i] = child.position;

            if(i > 0)
            {
                Vector3 delta = pathPoints[i] - pathPoints[i - 1];
                totalPathLength += delta.magnitude;

                if(i > loopRestartIndex)
                {
                    loopPathLength += delta.magnitude;
                }

                Debug.Log("totalPathLength: " + totalPathLength + " loopPathLength: " + loopPathLength);
            }

//            Debug.Log("Point: " + i + " pos: " + pathPoints[i] + " name: " + child.name + " totalPathLength: " + totalPathLength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( Cursor != null && Cursor.activeInHierarchy )
            UpdateCursor();
    }

    private void UpdateCursor()
    {
        float useSpeed = CursorSpeed * Time.deltaTime;

        cursorProgress += useSpeed;

        var (pos, complete) = GetPointFromProgress(cursorProgress);
        Cursor.transform.position = pos;
    }

    public (Vector3 position, bool complete) GetPointFromProgress(float progress)
    {
        bool pathComplete = false;

        int startIndex = 0;
        if(progress > totalPathLength)
        {
            pathComplete = true;

            progress -= totalPathLength;
            progress %= loopPathLength;
            startIndex = loopRestartIndex;
        }

//        Debug.Log("progress: " + progress + " startIndex: " + startIndex);

        float cumulativeDist = 0;

        Vector3 result = new Vector3();

        for (int i = startIndex; i < pathPoints.Length - 1; i++)
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

        return (result, pathComplete);
    }
}
