using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int arrowSpeed;
    public float maxRange = 5f;  // Maximale afstand voor de crosshair vanaf de boog
    private GameObject crossHair;
    private bool isCrossHairActive = false;  // Of de crosshair actief is
    private Animator animator;  // Animator voor de player

    void Start()
    {
        // Zoek naar de crosshair object in de scene (moet de tag 'Crosshair' hebben)
        crossHair = GameObject.FindGameObjectWithTag("crossHair");
        if (crossHair != null)
        {
            crossHair.SetActive(false);  // Start zonder crosshair
        }

        // Verkrijg de Animator component van de speler (moet aan de speler zelf toegevoegd zijn)
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Toggle de crosshair en cursor als op de B-toets wordt gedrukt
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleCrossHair();
        }

        // Schiet een pijl wanneer de linkermuisknop wordt ingedrukt
        if (Input.GetMouseButtonDown(0) && isCrossHairActive)
        {
            animator.SetBool("shooting", true);
            ShootArrow();
            animator.SetBool("shooting", true);  // Zet de shooting boolean aan
        }
        else
        {
            animator.SetBool("shooting", false);  // Zet de shooting boolean uit als niet schiet
        }

        // Beweeg de crosshair naar de muispositie als deze actief is
        if (isCrossHairActive)
        {
            MoveCrossHair();
            UpdateShootingDirection();  // Update de richting op basis van de crosshair
        }
    }

    private void ToggleCrossHair()
    {
        // Wissel de toestand van de crosshair
        isCrossHairActive = !isCrossHairActive;

        // Zet de crosshair in/uit beeld
        if (crossHair != null)
        {
            crossHair.SetActive(isCrossHairActive);
        }

        // Toon of verberg de cursor
        Cursor.visible = !isCrossHairActive;
    }

    private void ShootArrow()
    {
        // Verkrijg de muispositie en zet deze om naar wereldcoördinaten
        Vector3 targetPosition = crossHair.transform.position;

        // Maak een pijl en positioneer deze bij de boog
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // Bereken de richting van de pijl naar de crosshair
        Vector2 direction = (targetPosition - transform.position).normalized;

        // Draai de pijl zodat deze naar de crosshair wijst
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Zet de snelheid van de pijl in de richting van de crosshair
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * arrowSpeed;
    }

    private void MoveCrossHair()
    {
        // Verkrijg de muispositie in wereldcoördinaten
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;  // Zet de z-waarde op 0 zodat de crosshair zich op het 2D-vlak bevindt

        // Bereken de afstand tussen de boog en de muis
        Vector3 directionToMouse = mousePosition - transform.position;

        // Beperk de crosshair naar het maximaal bereik
        if (directionToMouse.magnitude > maxRange)
        {
            // Normaliseer de richting zodat de crosshair binnen het bereik blijft
            directionToMouse = directionToMouse.normalized * maxRange;
        }

        // Beweeg de crosshair naar de nieuwe positie
        crossHair.transform.position = transform.position + directionToMouse;
    }

    private void UpdateShootingDirection()
    {
        // Verkrijg de richting van de crosshair ten opzichte van de boog
        Vector3 directionToCrossHair = crossHair.transform.position - transform.position;

        // Bereken de hoek van de crosshair ten opzichte van de speler
        float angle = Mathf.Atan2(directionToCrossHair.y, directionToCrossHair.x) * Mathf.Rad2Deg;

        // Bepaal de richting op basis van de hoek en zet de juiste animatieparameter
        if (angle >= -45f && angle < 45f)
        {
            animator.SetFloat("moveX", 1f);  // Schieten naar rechts
            animator.SetFloat("moveY", 0f);  // Geen verticale beweging
                                             // Zet de speler niet gespiegeld voor rechts
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (angle >= 45f && angle < 135f)
        {
            animator.SetFloat("moveX", 0f);  // Geen horizontale beweging
            animator.SetFloat("moveY", 1f);  // Schieten omhoog
                                             // Zet de speler niet gespiegeld voor omhoog
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (angle >= 135f || angle < -135f)
        {
            animator.SetFloat("moveX", -1f);  // Schieten naar links
            animator.SetFloat("moveY", 0f);  // Geen verticale beweging
                                             // Spiegel de speler voor links
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (angle <= -45f && angle > -135f)
        {
            animator.SetFloat("moveX", 0f);  // Geen horizontale beweging
            animator.SetFloat("moveY", -1f);  // Schieten naar beneden
                                              // Zet de speler niet gespiegeld voor naar beneden
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
