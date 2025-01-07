using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public string interactionPrompt = ""; // Tekst die aangeeft dat je kunt interacten
    public string interactionTextFirst = ""; // De tekst die wordt weergegeven bij interactie
    public string interactionTextSecond = ""; // De tekst die wordt weergegeven na spatie
    public GameObject interactionPromptCanvas; // Canvas voor de prompt ("Press E to interact")
    public GameObject interactionDetailCanvas; // Canvas voor de detailtekst
    private bool isPlayerInRange = false; // Controleert of de speler in de triggerzone is
    private NPCmovement npcMovement; // Referentie naar het NPCmovement-script
    public Animator anim; // Animator van de NPC
    private bool isDetailVisible = false; // Controleert of de detailtekst zichtbaar is
    private bool isSecondTextVisible = false; // Houdt bij of de tweede tekst wordt getoon
    public Quest TalkToBobQuest;
    public QuestManager QuestManager;
    public bool IsInteracting;

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
                QuestManager.AddQuest(TalkToBobQuest);
                IsInteracting = true;
            }

            // Als detailtekst zichtbaar is, luister naar spatie
            if (isDetailVisible && Input.GetKeyDown(KeyCode.R))
            {
                ShowNextText();
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
            interactionDetailCanvas.SetActive(false);
            isDetailVisible = false;
            isSecondTextVisible = false;

            // Stop NPC interactie indien nodig
            npcMovement?.StopInteraction();
        }
        else
        {
            // Toon de eerste detailtekst
            interactionPromptCanvas.SetActive(false);
            interactionDetailCanvas.SetActive(true);
            interactionDetailCanvas.GetComponentInChildren<TextMeshProUGUI>().text = interactionTextFirst;

            isDetailVisible = true;
            isSecondTextVisible = false;

            // Start NPC interactie
            npcMovement?.StartInteraction();
        }
    }

    private void ShowNextText()
    {
        if (!isSecondTextVisible)
        {
            // Toon de tweede tekst
            interactionDetailCanvas.GetComponentInChildren<TextMeshProUGUI>().text = interactionTextSecond;
            isSecondTextVisible = true;
        }
        else
        {
            // Sluit de interactie als spatie opnieuw wordt gedrukt
            ToggleDetailText();
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
            isSecondTextVisible = false; // Reset de tweede tekst
            anim?.SetBool("moving", true); // Start NPC animatie
            npcMovement?.StopInteraction(); // Start NPC beweging
            IsInteracting = false;
        }
    }
}
