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
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    private Animator animator;
    public GameObject deathEffect;

    [Header("Item Drops")]
    public GameObject heartPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.25f;

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
            TryDropItem();
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

    private void TryDropItem()
    {
        if (heartPrefab != null)
        {
            float randomValue = Random.value;
            if (randomValue <= dropChance)
            {
                Instantiate(heartPrefab, transform.position, Quaternion.identity);
            }
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
}
