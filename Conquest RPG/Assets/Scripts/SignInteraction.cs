using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInteraction : MonoBehaviour
{
    public string defaultText = "Welcome to the sign!"; // De standaard tekst
    public string interactionText = "You pressed E! Here's more info."; // De tekst na interactie

    // Canvas 1: Trigger-zone
    public GameObject canvasTriggerZone; // Canvas voor tekst als de speler in de triggerzone komt
    public TMP_Text triggerZoneText; // TMP-component voor de triggerzone

    // Canvas 2: Interactie
    public GameObject canvasInteraction; // Canvas voor tekst en afbeelding bij interactie
    public TMP_Text interactionTextUI; // TMP-component voor interactie
    public Image interactionImage; // Afbeelding die zichtbaar wordt bij interactie

    private bool isPlayerInRange = false; // Controleert of de speler in de triggerzone is
    private bool hasInteracted = false; // Controleert of er al is geïnteracteerd

    private void Start()
    {
        // Verberg beide canvassen bij het begin
        if (canvasTriggerZone != null)
            canvasTriggerZone.SetActive(false);

        if (canvasInteraction != null)
            canvasInteraction.SetActive(false);
    }

    private void Update()
    {
        // Toon standaardtekst in trigger-zone canvas als de speler in de triggerzone is
        if (isPlayerInRange && !hasInteracted && canvasTriggerZone != null)
        {
            ShowTriggerZoneUI();
        }

        // Controleer op interactie met de speler
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // Verberg de triggerzone-UI en toon de interactie-UI
        if (canvasTriggerZone != null)
            canvasTriggerZone.SetActive(false);

        if (canvasInteraction != null)
        {
            canvasInteraction.SetActive(true);
            interactionTextUI.text = interactionText;
            interactionImage.enabled = true; // Zorg dat de afbeelding zichtbaar is
        }

        hasInteracted = true; // Markeer interactie als voltooid
        Debug.Log("De speler heeft geïnteracteerd met het bord.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Controleer of de speler het triggergebied binnenkomt
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Speler is in de triggerzone van het bord.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Controleer of de speler het triggergebied verlaat
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Verberg beide canvassen
            if (canvasTriggerZone != null)
                canvasTriggerZone.SetActive(false);

            if (canvasInteraction != null)
                canvasInteraction.SetActive(false);

            hasInteracted = false; // Reset interactie bij verlaten van de triggerzone
            Debug.Log("Speler heeft de triggerzone van het bord verlaten.");
        }
    }

    private void ShowTriggerZoneUI()
    {
        if (canvasTriggerZone != null)
        {
            canvasTriggerZone.SetActive(true);
            triggerZoneText.text = defaultText;
        }
    }
}
