using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Knockback : MonoBehaviour
{
    public Projectile Projectile;
    public float thrust;
    public float knockTime;
    public float damage;

    public GameObject entity;

    private IEnumerator ResetVelocity(Rigidbody2D hit, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hit != null)
        {
            hit.linearVelocity = Vector2.zero; // Stop de beweging
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Logica voor enemyprojectile die de speler raakt
        if (this.gameObject.CompareTag("enemyProjectile") && other.gameObject.CompareTag("Player"))
        {
                Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
                if (hit != null)
                {
                    Vector2 difference = hit.transform.position - entity.transform.position;
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                    Destroy(this.gameObject);
                    StartCoroutine(ResetVelocity(hit, knockTime)); // Verwijder kracht na knockTime
                    if (other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                        other.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }
                return; // Stop verdere verwerking voor deze tag
            }

            // Voorkom dat player-aanvallen enemyprojectile beïnvloeden
            if (this.gameObject.CompareTag("Player") && other.gameObject.CompareTag("enemyProjectile"))
            {
                return; // Niets doen als de player een enemyprojectile raakt
            }

            // Originele logica voor andere interacties
            if (!other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("arrow") || this.gameObject.CompareTag("enemy") && !other.gameObject.CompareTag("enemy")
                || this.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
                {
                    //other.GetComponent<pot>().Smash();
                }
                if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
                {
                    Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
                    if (hit != null)
                    {
                        Vector2 difference = hit.transform.position - entity.transform.position;
                        difference = difference.normalized * thrust;
                        hit.AddForce(difference, ForceMode2D.Impulse);
                        StartCoroutine(ResetVelocity(hit, knockTime)); // Verwijder kracht na knockTime
                        if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                        {
                            if (hit.GetComponent<Enemy>().currentState != EnemyState.stagger)
                            {
                                hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                            }
                            other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                        }
                        if (other.gameObject.CompareTag("Player"))
                        {
                            if (other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                            {
                                hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                                other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                            }
                        }
                    }
                }
            }
        

    }
}