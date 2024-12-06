using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;

    // Voeg quest toe aan de lijst
    public void AddQuest(Quest quest)
    {
        // Alleen toevoegen als de quest nog niet in de lijst zit
        if (!quests.Contains(quest))
        {
            quests.Add(quest);
        }
    }

    // Maak een method om quests als voltooid te markeren
    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
    }
}