using UnityEngine;

public class Skeleton_Swordman : Enemy
{
    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;

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
        //if (currentState != EnemyState.dying)
        {
            CheckDistance();
            
        }
        //else if(currentState == EnemyState.dead)
        //{
        //    this.gameObject.SetActive(false);
        //}
        //else
        //{
        //    anim.SetBool("dying", true);

        //    currentState = EnemyState.dead;

        //}
    }

    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {

                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("moving", true);
                //anim.SetBool("wakeUp", true);
                anim.SetBool("attacking", false);
            }

        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            anim.SetBool("attacking", true);
        }

        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            //anim.SetBool("wakeUp", false);
            anim.SetBool("moving", false);
            anim.SetBool("attacking", false);
        }
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

