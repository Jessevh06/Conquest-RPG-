using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public QuestManager questManager;    // Verwijzing naar de QuestManager
    public TextMeshProUGUI questText;    // Voor de naam en status
    public TextMeshProUGUI descriptionText; // Voor de beschrijving met kleiner lettertype

    void Update()
    {
        string questDisplayText = "| Active Quests |\n";
        string descriptionDisplayText = ""; // Voor de beschrijvingen

        foreach (Quest quest in questManager.quests)
        {
            if (!quest.isCompleted)
            {
                string questStatus = "In Progress";
                questDisplayText += $"{quest.questName} - {questStatus}\n";

                // Beschrijving apart toevoegen
                descriptionDisplayText += $"Description: {quest.description}\n";
            }
        }

        // Update de UI
        questText.text = questDisplayText;
        descriptionText.text = descriptionDisplayText;
    }
}
