using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public QuestManager questManager;  // Verwijzing naar QuestManager
    public Quest[] questsToGive;  // De quests die deze questgever biedt
    public GameObject questUI;  // Canvas voor de QuestUI
    private bool isPlayerInRange = false;  // Controleert of de speler in de triggerzone is
    public NPCInteraction NPCInteraction;

    private void Start()
    {
        questUI.SetActive(false);
    }

    public void EnableQuestUI() 
        {
            questUI.SetActive(true);
        }
    private void GiveQuests()
    {
        foreach (Quest quest in questsToGive)
        {
            // Voeg quests toe aan de QuestManager
            if (!quest.isCompleted && !questManager.quests.Contains(quest))
            {
                quest.isCompleted = false; // Zet de status op "In Progress"
                questManager.AddQuest(quest);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
