using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class chest : MonoBehaviour
{
    public Animator chestAnimator;
    public GameObject swordPrefab;
    public Transform itemSpawnPoint;
    public KeyCode interactKey = KeyCode.E;
    public TMP_Text PickUpMessage;
    public float messageDuration = 3f;
    public GameObject interactText;

    private bool isPlayerNearby = false;
    private bool isChestOpen = false;
    

    private PlayerMovement playerMovement;

    private void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        if (PickUpMessage != null)
        {
            PickUpMessage.gameObject.SetActive(false);
        }

       

      
    }

    void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(interactKey) && !isChestOpen)
        {
            Debug.Log("Interactie; Open de kist.");
            OpenChest();
        }

       
    }

    void OpenChest()
    {
        if (!isChestOpen)
        {
            isChestOpen = true;
            
            if (chestAnimator != null)
            {
                Debug.Log("Trigger 'OpenChest' wordt gezet");
                chestAnimator.SetTrigger("ChestOpen");

                Debug.Log("Huidige Animator State: " + chestAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash);
            }
            else
            {
                Debug.LogError("chestAnimator is niet gekoppeld");
            }

            if (interactText != null)
            {
                interactText.SetActive(false);
            }
            SpawnItem();

            StartCoroutine(OpenChestSequence());
        }       
    }

    void SpawnItem()
    {
        if (swordPrefab != null && itemSpawnPoint != null)
        {

            Vector3 spawnPosition = itemSpawnPoint.position;

            GameObject spawnedSword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);

            Debug.Log("Zwaard gespawned op: " + spawnPosition);

            ShowPickUpMessage("You found a Sword!");

            if (playerMovement != null)
            {
                playerMovement.SetCanAttack(true);
                playerMovement.currentState = PlayerState.walk;
            }
            if (PickUpMessage != null)
            {
                PickUpMessage.text = "You have found a Sword!";
                PickUpMessage.gameObject.SetActive(true);
            }
           
            

            Destroy(spawnedSword, 2f);
        }
        else
        {
            Debug.LogWarning("SwordPrefab is niet ingesteld!");
        }
    }



    void ShowPickUpMessage(string message)
    {
        if (PickUpMessage != null)
        {
            // Stel de TMP-tekst in
            PickUpMessage.text = message;

            // Activeer de TMP-tekst
            PickUpMessage.gameObject.SetActive(true);

            // Verberg de TMP-tekst na een paar seconden
            Invoke(nameof(HidePickUpMessage), messageDuration);
        }
        else
        {
            Debug.LogWarning("PickUpMessage (TMP) is niet gekoppeld aan het script!");
        }
    }

    void HidePickUpMessage()
    {
        if (PickUpMessage != null)
        {
            // Verberg de TMP-tekst
            PickUpMessage.gameObject.SetActive(false);
        }
    }







    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is dichtbij de kist.");
            isPlayerNearby = true;
            if (interactText != null)
            {
                interactText.SetActive(true);
            }

            playerMovement = other.GetComponent<PlayerMovement>();
            
            if (playerMovement != null)
            {
                playerMovement.currentState = PlayerState.idle;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Speler is uit de buurt van de kist.");
            isPlayerNearby = false;
            if (interactText != null)
            {
                interactText.SetActive(false);
            }

            if (playerMovement != null)
            {
                playerMovement.currentState = PlayerState.walk;

            }
        }
    }

    //IEnumerator StopAnimatorAfterDelay(Animator animator, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    animator.enabled = false;

    //}

    IEnumerator OpenChestSequence()
    {
        Debug.Log("Wacht op de animatie...");
        yield return new WaitForSeconds(1f);

        Debug.Log("Zwaard wordt gespawned");
        SpawnItem();
    }
}

    