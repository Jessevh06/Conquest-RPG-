using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    
    public float floatSpeed = 0.1f;
    public float maxHeight = 2.0f;

    private Vector3 startPosition;
    private bool isFloating = false;
    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        if (isFloating && transform.position.y < startPosition.y + maxHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + floatSpeed, transform.position.z);
        }   
    }

    public void StartFloating()
    {
        isFloating = true;
    }
}
