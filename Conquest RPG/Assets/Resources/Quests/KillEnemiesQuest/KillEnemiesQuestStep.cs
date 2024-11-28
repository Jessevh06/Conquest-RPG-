using UnityEngine;

public class KillEnemiesQuestStep : QuestStep
{
    private int enemiesKilled = 0;
    private int enemiesKilledToComplete = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onEnemiesKilled += enemiesKilled;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onEnemyKilled -= enemiesKilled;
    }

    private void EnemiesKilled()
    {
        if (enemiesKilled < enemiesKilledToComplete)
        {
            enemiesKilled++;
        }

        if (enemiesKilled > enemiesKilledToComplete)
        {
            FinishQuestStep();
        }
    }
}
