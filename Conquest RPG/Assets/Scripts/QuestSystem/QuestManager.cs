using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public BoolValue swordPickedUp;
    public Quest SwordPickUpQuest;
    public BoolValue bowPickedUp;
    public Quest BowPickUpQuest;
    public Quest TalkToBobQuest;
    public Quest GoBackToBobQuest;
    public Quest KillEnemiesQuest;
    public NPCInteraction NPCInteraction;

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

    public void Update()
    {
        if (swordPickedUp.initialValue)
        {
            CompleteQuest(SwordPickUpQuest);
          
        }

        if (TalkToBobQuest && NPCInteraction.IsInteracting)
        {
            CompleteQuest(TalkToBobQuest);
            AddQuest(BowPickUpQuest);
        }
      
        if (bowPickedUp.initialValue)
        {
            CompleteQuest(BowPickUpQuest);
        }

        if (GoBackToBobQuest.isCompleted)
        {
            AddQuest(KillEnemiesQuest);
        }
    }
}
