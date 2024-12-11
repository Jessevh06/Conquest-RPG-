using UnityEngine;
using System.Collections.Generic;
using TMPro;
using JetBrains.Annotations;

public class EnemyCounter : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name;
        public int currentCount;
        public int totalCount;
    }

    public List<EnemyType> enemyTypes;
    public TextMeshProUGUI enemyCounterText;

    void Start()
    {
        UpdateCounterUI();
    }

    public void EnemyDefeated(string enemyName)
    {
        foreach (var enemyType in enemyTypes)
        {
            if (enemyType.name == enemyName)
            {
                enemyType.currentCount++;
                break;
            }
        }
        UpdateCounterUI();
    }

    private void UpdateCounterUI()
    {
        if (enemyCounterText == null) return;

        enemyCounterText.text = "";
        foreach (var enemyType in enemyTypes)
        {
            enemyCounterText.text += $"{enemyType.currentCount}/{enemyType.totalCount} {enemyType.name}\n";
        }
    }

   
}
