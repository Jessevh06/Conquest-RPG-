using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;

    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }

        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.1f);
        currentState = PlayerState.walk;
    }
    //void UpdateAnimationAndMove()
    //{
    //    if (change != Vector3.zero)
    //    {
    //        MoveCharacter();
    //        animator.SetFloat("moveX", change.x);
    //        animator.SetFloat("moveY", change.y);
    //        animator.SetBool("moving", true);

    //    }
    //    else
    //    {
    //        animator.SetBool("moving", false);
    //    }
    //}


    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();

            // Update de animator parameters
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);

            // Spiegel het object als het naar links beweegt
            if (change.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Spiegel op de X-as
            }
            else if (change.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Normaal op de X-as
            }
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    //

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
            );

    }
    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }//
    private IEnumerator KnockCo(float knockTime)
    {
       
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.linearVelocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.linearVelocity = Vector2.zero;
        }
    }

    
}

