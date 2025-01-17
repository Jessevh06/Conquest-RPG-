using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnvoirmentLayers : MonoBehaviour
{
    private GameObject player; // Referentie naar de speler
    private float playerY;     // Variabele om de Y-positie van de speler op te slaan
    private float objectY;
    private float colliderY;
    private SpriteRenderer spriteRenderer;
    private float centerY;



    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (polygonCollider != null)
        {
            colliderY = polygonCollider.transform.position.y;
            Bounds bounds = polygonCollider.bounds;
            centerY = bounds.center.y;
        }
        else if (boxCollider != null)
        {
            colliderY = boxCollider.transform.position.y;
            Bounds bounds = boxCollider.bounds;
            centerY = bounds.center.y;
            Debug.Log(centerY);
        }
        else if (capsuleCollider != null)
        {
            colliderY = capsuleCollider.transform.position.y;
            Bounds bounds = capsuleCollider.bounds;
            centerY = bounds.center.y;

        }
        else
        {
            Debug.Log("geen collider gevonden");
        }
    }
        void Update()
        {
            if (player != null)
            {

                playerY = player.transform.position.y;
                playerY = playerY - 0.25f;
                objectY = this.transform.position.y;


                if (centerY > playerY)
                {
                    spriteRenderer.sortingOrder = 5;
                }
                else if (centerY < playerY)
                {
                    spriteRenderer.sortingOrder = 7;
                }


            }
            else
            {
                Debug.LogWarning("Speler niet gevonden!");
            }
        }
}

