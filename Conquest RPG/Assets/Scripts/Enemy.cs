using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
    //dying,
    //dead
}

public class Enemy : MonoBehaviour
{
    private static Enemy instance;
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    private Animator animator;
    public GameObject deathEffect;
    public FloatValueChangeAble totalEnemiesBrokenCity;
    

    private void Awake()
    {
        health = maxHealth.initialValue;
        
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //currentState = EnemyState.dying;
            //animator.SetBool("dying", true);
            DeathEffect();
            totalEnemiesBrokenCity.initialValue --;
            this.gameObject.SetActive(false);
        }
        
    }
    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }
    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.linearVelocity = Vector2.zero;
            myRigidbody.GetComponent<Enemy>().currentState = EnemyState.idle;
            myRigidbody.linearVelocity = Vector2.zero;

        }
    }
}//
