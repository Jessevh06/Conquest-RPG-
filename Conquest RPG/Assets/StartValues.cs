using UnityEngine;

public class StartValues : MonoBehaviour
{
    private Quest Quest;
    public Quest SwordPickupQuest;
    public Quest BowPickupQuest;
    public Quest GoBackToBobQuest;
    public Quest TalkToBobQuest;
    public Quest KillEnemiesQuest;
    public bool BowPickedup;
    public bool SwordPickedup;


    void Start()
    {
        BowPickedup = false;
        SwordPickedup = false;
        SwordPickupQuest.isCompleted = false;
        BowPickupQuest.isCompleted = false;
        GoBackToBobQuest.isCompleted = false;
        TalkToBobQuest.isCompleted = false;
        KillEnemiesQuest.isCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
