using UnityEngine;
using System.Collections;

public class Skeleton_Bowman : MonoBehaviour
{
    public Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 1f;
    public int towerDamage;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Animator anim;
    public Vector2 direction;
    private bool isFacingLeft = false; // Houd bij of de bowman naar links kijkt

    private void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = nearestEnemy != null && shortestDistance <= range ? nearestEnemy.transform : null;
    }

    void Update()
    {
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
            bullet.SetDamage(towerDamage);
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
}
