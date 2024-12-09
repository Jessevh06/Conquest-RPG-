using UnityEngine;
using System.Collections;

public class NPCmovement : MonoBehaviour
{
    public float speed = 1f; // Snelheid van de NPC
    private Transform target; // Huidig doel
    private int waypointIndex = 0; // Index van huidige waypoint
    public Animator anim; // Animator voor animaties
    private Vector2 previousPosition; // Positie in de vorige frame
    private bool isFacingLeft;
    private Vector2 direction;
    public float pauseTime = 1f; // Tijd in seconden om te pauzeren bij een waypoint
    private bool isPaused = false; // Controleert of de NPC gepauzeerd is
    private bool isInteracting = false; // Controleert of de NPC in interactie is
    private bool isInTriggerZone = false; // Controleert of de speler in de triggerzone is
    private bool isWaitingAtWaypoint = false; // Controleert of de NPC wacht bij het waypoint

    private void Start()
    {
        previousPosition = transform.position;
        anim.SetBool("moving", true);
        if (NPCPath.points.Length > 0)
        {
            target = NPCPath.points[0];
        }
        else
        {
            Debug.LogError("No waypoints found! Ensure NPCPath is set up correctly.");
        }
    }//

    private void Update()
    {
        if (isInteracting || isPaused) return; // Stop beweging bij interactie of pauze

        NPCUpdate(); // Update beweging
        UpdateAnimationAndFlip(); // Update animatie en spiegeling

        // Controleer interactie via de "E" toets als de speler in de triggerzone is
        if (isInTriggerZone && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction(); // Stop animatie bij interactie met 'E'
        }
    }

    private void NPCUpdate()
    {
        if (target == null) return;

        // Bepaal de richting naar het huidige waypoint
        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Controleer of de NPC bijna bij het waypoint is
        if (Vector2.Distance(transform.position, target.position) <= 0.1f && !isWaitingAtWaypoint)
        {
            StartCoroutine(PauseAtWaypoint());
        }
    }

    private void GetNextWaypoint()
    {
        // Controleer of de NPC het laatste waypoint heeft bereikt
        if (waypointIndex >= NPCPath.points.Length - 1)
        {
            Destroy(gameObject); // Verwijder de NPC
            return;
        }

        waypointIndex++; // Ga naar het volgende waypoint
        target = NPCPath.points[waypointIndex];
    }

    private void UpdateAnimationAndFlip()
    {
        if (isInteracting) return; // Stop animatie-updates tijdens interactie

        // Als de NPC zich verplaatst, bereken dan de richting naar het doel
        direction = (target.position - transform.position).normalized;

        // Pas de animatie aan op basis van de richting
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);

        // Als de NPC stilstaat, kijk dan naar de speler
        if (Vector2.Distance(transform.position, target.position) <= 0.1f && !isInteracting)
        {
            //LookAtPlayer();
        }
        else
        {
            // Ga verder met de NPC bewegen zoals normaal
            if (direction.x < 0 && !isFacingLeft)
            {
                Flip(true);
            }
            else if (direction.x > 0 && isFacingLeft)
            {
                Flip(false);
            }
        }
    }

    //private void LookAtPlayer()
    //{
    //    if (target != null && GameObject.FindWithTag("Player") != null)
    //    {
    //        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
    //        Vector2 directionToPlayer = playerPosition - transform.position;

    //        // Draai de NPC richting de speler
    //        if (directionToPlayer.x < 0 && !isFacingLeft)
    //        {
    //            Flip(true);
    //        }
    //        else if (directionToPlayer.x > 0 && isFacingLeft)
    //        {
    //            Flip(false);
    //        }
    //    }
    //}

    void Flip(bool faceLeft)
    {
        isFacingLeft = faceLeft;
        Vector3 scale = transform.localScale;
        scale.x = faceLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private IEnumerator PauseAtWaypoint()
    {
        
        isWaitingAtWaypoint = true; // Zorg dat de NPC wacht
        anim.SetFloat("moveX", 0); // Stop animatie
        anim.SetFloat("moveY", 0);
        anim.SetBool("moving", false); // Zorg dat de animatie stopt
        yield return new WaitForSeconds(pauseTime); // Wacht de pauzetijd

        GetNextWaypoint(); // Ga naar het volgende waypoint
        isWaitingAtWaypoint = false; // Hervat beweging
        anim.SetBool("moving", true);
    }

    // Start interactie en stop beweging
    public void StartInteraction()
    {
        isInteracting = true;
        anim.SetFloat("moveX", 0); // Stop animatie
        anim.SetFloat("moveY", 0);
        anim.SetBool("moving", false); // Stop beweging
        Debug.Log("NPC stopt beweging bij interactie");
    }

    // Stop interactie en hervat beweging
    public void StopInteraction()
    {
        isInteracting = false;
        anim.SetBool("moving", true); // Hervat beweging
        Debug.Log("NPC hervat beweging na interactie");
    }

    // Triggerzone enter en exit om interactie te controleren
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
            Debug.Log("Speler in triggerzone.");

            // Zorg ervoor dat de NPC naar de speler kijkt zodra deze in de triggerzone komt
            //LookAtPlayer();
            anim.SetBool("moving", false); // Stop beweging en animatie als speler in triggerzone is
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            Debug.Log("Speler verliet triggerzone.");

            // Begin de loopanimatie en beweging zodra de speler uit de triggerzone is
            anim.SetBool("moving", true);
            anim.SetFloat("moveX", 1); // Start de loop animatie (pas dit aan afhankelijk van jouw animatie)
        }
    }
}
