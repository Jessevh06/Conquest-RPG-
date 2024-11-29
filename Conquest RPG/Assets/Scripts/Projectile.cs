using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;
    public int bulletDamage;
    private Vector2 dir;

    public void Chase(Transform _target)
    {
        if (_target != null)
        {
            target = _target;
            dir = target.position - transform.position; // Richting initialiseren
            UpdateRotation();
        }
        else
        {
            Debug.LogWarning("Target is null!");
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        UpdateRotation();
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }

    private void UpdateRotation()
    {
        // Rotatie van de pijl instellen
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

