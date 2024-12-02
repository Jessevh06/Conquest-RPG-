using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    
    public GameObject entity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("arrow") || this.gameObject.CompareTag("enemyProjectile") && !other.gameObject.CompareTag("enemy")
            || this.gameObject.CompareTag("Player") || this.gameObject.CompareTag("enemy") && !other.gameObject.CompareTag("enemy"))
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
                    if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                    {
                        
                       hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
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
                    if (other.gameObject.CompareTag("enemyProjectile") && other.isTrigger)
                    {
                        this.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }

                }

            }
        }
    }
   
}
