using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Chicken : Animals
{
    private Animator anim;
    private Vector2 previousPosition;
    private Vector2 movement;
    public Vector2 direction;

    public bool isPicking = false;
    
    public bool isWalking = true;

    private float stopDuration = 2f;
    private float walkDuration = 3f;
    private float timer = 0f;
    private bool isStopped = false;
    private bool transitionToPicking = false;
    private bool transitionToWalking = false;

    private void Start()
    {
        SetNewDestination();
        anim = GetComponent<Animator>();
        previousPosition = transform.position;
        anim.SetBool("Walking", true);
        anim.SetBool("Picking", false);
        anim.SetBool("Idle", false);
        
    }

    private void Update()
    {
        anim.SetBool("Idle", false);
        timer += Time.deltaTime;

        if (transitionToPicking)
        {
            if (timer >= 0.5f)
            {
                transitionToPicking = false;
                isStopped = true;
                timer = 0f;
                anim.SetBool("Picking", true);
                anim.SetBool("Idle", false);
            }
        }
        else if (transitionToWalking)
        {
            if (timer >= 0.5f)
            {
                transitionToWalking = false;
                isStopped = false;
                anim.SetBool("Walking", true);
                anim.SetBool("Idle", false);
                SetNewDestination();
            }
        }
        else if (isStopped)
        {
            if (timer >= stopDuration)
            {
                anim.SetBool("Picking", false);
                anim.SetBool("Idle", false);
                transitionToWalking = true;
                timer = 0f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, wayPoint) < range || timer >= walkDuration)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Walking", false);
                transitionToPicking = true;
                timer = 0f;
            }
        }
        UpdateAnimation();









    }
    public void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movementDirection = currentPosition - previousPosition;

        if (!isStopped && movementDirection.magnitude > 0.0000001f)
        {
            HandleDirection(movementDirection);
        }


        previousPosition = currentPosition;

        
        
    }

    private void HandleDirection(Vector2 direction)
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


}
