using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 PlayerPosition;
    public VectorValue playerStorage;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.initialValue = PlayerPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
