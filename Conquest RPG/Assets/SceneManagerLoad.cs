using UnityEngine;

public class SceneManagerLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool BowPickedUp;
    public bool SwordPickedUp;
    public Quest BowPickedUpQuest;
    public Quest SwordPickedUpQuest;
    public Quest KillEnemiesQuest;
    public Quest TalkToBobQuest;
    private Quest quest;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BowPickedUp = false;
        SwordPickedUp = false;
        SwordPickedUpQuest.isCompleted = false;
        BowPickedUpQuest.isCompleted = false;
        KillEnemiesQuest.isCompleted = false;
        TalkToBobQuest.isCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
