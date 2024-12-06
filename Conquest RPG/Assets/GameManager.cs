using UnityEngine;

public class GameManager : MonoBehaviour
{
    HeartManager Manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Manager.UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
