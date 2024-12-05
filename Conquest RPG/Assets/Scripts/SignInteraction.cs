using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInteraction : MonoBehaviour
{
    public string defaultText = "Welcome to the sign!"; // De standaard tekst
    public string interactionText = "You pressed E! Here's more info."; // De tekst na interactie
    public GameObject textUI; // Referentie naar de tekst (TMP component)
    public Image backgroundImage; // Referentie naar de Image van de Canvas
    private bool isPlayerInRange = false; // Controleert of de speler in de triggerzone is
    private bool hasInteracted = false; // Controleert of er al is geïnteracteerd

    private void Start()
    {
        // Verberg tekst en achtergrond bij het begin
        if (textUI != null)
        {
            textUI.SetActive(false);
        }

        if (backgroundImage != null)
        {
            backgroundImage.enabled = false; // Zorg dat de Image niet zichtbaar is
        }
    }

    private void Update()
    {
        // Toon standaardtekst en achtergrond als de speler in de triggerzone is
        if (isPlayerInRange && !hasInteracted)
        {
            ShowUI(defaultText);
        }

        // Controleer op interactie met de speler
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // Wijzig tekst bij interactie
        if (textUI != null)
        {
            textUI.GetComponent<TextMeshProUGUI>().text = interactionText;
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

            // Verberg tekst en achtergrond
            HideUI();

            hasInteracted = false; // Reset interactie bij verlaten van de triggerzone
            Debug.Log("Speler heeft de triggerzone van het bord verlaten.");
        }
    }

    private void ShowUI(string text)
    {
        if (textUI != null)
        {
            textUI.SetActive(true);
            textUI.GetComponent<TextMeshProUGUI>().text = text;
        }

        if (backgroundImage != null)
        {
            backgroundImage.enabled = true; // Toon de achtergrond
        }
    }

    private void HideUI()
    {
        if (textUI != null)
        {
            textUI.SetActive(false);
        }

        if (backgroundImage != null)
        {
            backgroundImage.enabled = false; // Verberg de achtergrond
        }
    }
}
