using UnityEngine;
using System.Collections;

public class Skeleton_Mage : Enemy
{
    public Transform target;
    private Rigidbody2D myRigidbody;
    public float chaseRadius;
    public float attackRadius;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 1f;
    

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Animator anim;
    public Vector2 direction;
    private bool isFacingLeft = false; // Houd bij of de bowman naar links kijkt

    private void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;

    }

    void Update()
    {
        CheckDistance();

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
            anim.SetBool("moving", false);
            //anim.SetBool("attacking", true);
            if (target == null)
            {
                fireCountdown = Mathf.Max(fireCountdown - Time.deltaTime, 0f);
                anim.SetBool("attacking", false); // Stop animatie
                return;
            }

            direction = (target.position - transform.position).normalized;

            // Update animatie richting
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);

            // Controleer of de bowman naar links of rechts kijkt
            if (direction.x < 0 && !isFacingLeft)
            {
                Flip(true); // Flip naar links
            }
            else if (direction.x > 0 && isFacingLeft)
            {
                Flip(false); // Flip naar rechts
            }

            if (fireCountdown <= 0f)
            {
                StartCoroutine(AttackCo());
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }

        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            //anim.SetBool("wakeUp", false);
            anim.SetBool("moving", false);
            anim.SetBool("attacking", false);
        }
    }
    private IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(0.5f); // Delay voor schieten
        Shoot();
        anim.SetBool("attacking", false);
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile bullet = bulletGO.GetComponent<Projectile>();
        if (bullet != null)
        {
            bullet.Chase(target);
            
        }
    }

    void Flip(bool faceLeft)
    {
        isFacingLeft = faceLeft;
        Vector3 scale = transform.localScale;
        scale.x = faceLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = target != null ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
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

