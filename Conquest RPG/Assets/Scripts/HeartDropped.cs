using UnityEngine;

public class HeartDropped : MonoBehaviour
{
    [Header("Healing Settings")]
    public int healingAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerHealth = collision.GetComponent<PlayerMovement>();

            if (playerHealth != null)
            {
                
                Destroy(this.gameObject);
            }
        }
    }

}
