using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Animals : MonoBehaviour
{
    public float walkRadius = 10f;
    public float moveSpeed = 1f;


    private Vector3 startPosition;
    protected Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AnimalStart()
    {
        startPosition = transform.position;
        SetNewTargetPosition();
    }

    // Update is called once per frame
    public void AnimalUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    protected void SetNewTargetPosition()
    {
        Vector3 newTarget;
        do
        {
            newTarget = new Vector3(
                Random.Range(transform.position.x - walkRadius, transform.position.x + walkRadius),
                transform.position.y,
                Random.Range(transform.position.z - walkRadius, transform.position.z + walkRadius)
                );
        } while (Vector3.Distance(transform.position, newTarget) < 1f);

        targetPosition = newTarget;

    }

    protected void MoveToTarget()
    {
        
        Vector3 currentPosition = transform.position;
        Vector3 target = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z);

        transform.position = Vector3.MoveTowards(currentPosition, target, moveSpeed * Time.deltaTime);

        Debug.Log($"Current Position: {transform.position}, Target Position: {targetPosition}");
          
        /*
        if (targetPosition == null)
        {
            SetNewTargetPosition();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            if (direction.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
            }
        }*/
    }
}
    


