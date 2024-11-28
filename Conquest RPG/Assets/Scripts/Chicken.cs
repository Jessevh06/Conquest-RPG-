using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Chicken : Animals
{

    private Animator animator;
    public Animals Animals;
    private bool isWalking = false;

    public float idleDuration = 3f;
    private float idleTimer;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animals.AnimalStart();
        animator = GetComponent<Animator>();
        SetNewTargetPosition();
       
    }

    // Update is called once per frame
    void Update()
    {
        Animals.AnimalUpdate();
        if (!isWalking)
        {

            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                StartWalking();
            }
        }
        else
        {
            MoveToTarget();

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < 0.2f)
            {
                Debug.Log("Reached target position");
                StopWalking();
            }
        }
            

        animator.SetBool("Walking", isWalking);
    }

    private void StartWalking()
    {
        isWalking = true;
        idleTimer = 0f;
        SetNewTargetPosition();

        
    }

    private void StopWalking()
    {
        isWalking = false;
        idleTimer = 0f;
        SetNewTargetPosition();
    }

    
}
