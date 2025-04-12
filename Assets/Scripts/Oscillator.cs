// Purpose: To create a simple oscillating movement for an object in Unity.
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float moveSpeed = 2f;
    Vector3 startPosition;
    Vector3 endPosition;
    float movementFactor = 0f;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;    
    }

    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
    
}
