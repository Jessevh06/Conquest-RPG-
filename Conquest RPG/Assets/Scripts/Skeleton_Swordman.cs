using UnityEngine;

public class Skeleton_Swordman : Enemy
{
    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;

    public float attackCooldown = 1.5f; // Tijd tussen aanvallen
    private float lastAttackTime = 0f;

    public float knockbackForce = 3f; // Hoe sterk de knockback is

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        float distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (distanceToTarget <= chaseRadius && distanceToTarget > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("moving", true);
                anim.SetBool("attacking", false);
            }
        }
        else if (distanceToTarget <= attackRadius)
        {
            StopMovement(); // Stop de vijand zodra hij binnen aanvalsbereik komt

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                anim.SetBool("attacking", true);
                ApplyKnockback();
                lastAttackTime = Time.time; // Reset de cooldown-timer
            }
        }
        else if (distanceToTarget > chaseRadius)
        {
            anim.SetBool("moving", false);
            anim.SetBool("attacking", false);
        }
    }

    private void StopMovement()
    {
        // Reset beweging en verander de status naar idle
        myRigidbody.linearVelocity = Vector2.zero;
        anim.SetBool("moving", false);
        ChangeState(EnemyState.idle);
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = (transform.position - target.position).normalized;
        myRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    private void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
        // Spiegel het object als het naar links beweegt
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Spiegel op de X-as
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Normaal op de X-as
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
