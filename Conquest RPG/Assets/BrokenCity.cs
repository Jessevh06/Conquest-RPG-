using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class BrokenCity : MonoBehaviour
{

    public FloatValueChangeAble totalEnemiesBrokenCity;
    public string sceneToLoad;
    public Vector2 PlayerPosition;
    public VectorValue playerStorage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (totalEnemiesBrokenCity.initialValue <= 0)
        {
            SceneManager.LoadScene(sceneToLoad);
            playerStorage.initialValue = PlayerPosition;
        }
    }
}
