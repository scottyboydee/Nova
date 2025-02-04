using UnityEngine;

public class TestConstantMovement : MonoBehaviour
{
    [SerializeField] private Vector3 movementPerSecond = new Vector3(1f, 0f, 0f);

    void Update()
    {
        transform.position += movementPerSecond * Time.deltaTime;
    }
}
