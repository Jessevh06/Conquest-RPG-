using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
        public string questName;   // De naam van de quest
        public string description; // Beschrijving van de quest
        public bool isCompleted;   // Of de quest is voltooid
}
