using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public string interactionPrompt = ""; // Tekst die aangeeft dat je kunt interacten
    public string interactionDetailText = ""; // De tekst die wordt weergegeven bij interactie
    public GameObject interactionPromptCanvas; // Canvas voor de prompt (Press E to interact)
    public GameObject interactionDetailCanvas; // Canvas voor de detailtekst
    private bool isPlayerInRange = false; // Controleert of de speler in de triggerzone is
    private NPCmovement npcMovement; // Referentie naar het NPCmovement-script
    public Animator anim; // Animator van de NPC
    private bool isDetailVisible = false; // Controleert of de detailtekst zichtbaar is
    public QuestGiver QuestGiver;

    private void Start()
    {
        // Verberg beide canvassen bij het begin
        if (interactionPromptCanvas != null)
        {
            interactionPromptCanvas.SetActive(false);
        }

        if (interactionDetailCanvas != null)
        {
            interactionDetailCanvas.SetActive(false);
        }

        // Verkrijg de NPCmovement component
        npcMovement = GetComponent<NPCmovement>();
    }

    private void Update()
    {
        // Controleer of de speler in de buurt is
        if (isPlayerInRange && interactionPromptCanvas != null)
        {
            // Toon de interactieprompt als detailtekst niet zichtbaar is
            if (!isDetailVisible)
            {
                interactionPromptCanvas.SetActive(true);
            }

            // Als de speler op "E" drukt, wissel tussen prompt en detailtekst
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDetailText();
            }
        }
    }

    private void ToggleDetailText()
    {
        if (interactionPromptCanvas == null || interactionDetailCanvas == null) return;

        if (isDetailVisible)
        {
            // Ga terug naar interactieprompt
            interactionPromptCanvas.SetActive(true);
            QuestGiver.EnableQuestUI();
            interactionDetailCanvas.SetActive(false);
            isDetailVisible = false;

            // Indien nodig, beëindig NPC interactie
            npcMovement?.StopInteraction();
        }
        else
        {
            // Toon de detailtekst
            interactionPromptCanvas.SetActive(false);
            interactionDetailCanvas.SetActive(true);
            interactionDetailCanvas.GetComponentInChildren<TextMeshProUGUI>().text = interactionDetailText;
            isDetailVisible = true;

            // Start NPC interactie
            npcMovement?.StartInteraction();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPromptCanvas?.SetActive(true); // Toon prompt
            anim?.SetBool("moving", false); // Stop NPC animatie
            npcMovement?.StartInteraction(); // Stop NPC beweging
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPromptCanvas?.SetActive(false); // Verberg prompt
            interactionDetailCanvas?.SetActive(false); // Verberg detailtekst
            isDetailVisible = false; // Reset de detailtekst
            anim?.SetBool("moving", true); // Start NPC animatie
            npcMovement?.StopInteraction(); // Start NPC beweging
        }
    }
}
