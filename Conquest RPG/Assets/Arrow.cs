using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float despawnDistance = 10f;  // Maximale afstand die de pijl kan afleggen voordat hij verdwijnt
    private Vector3 startPosition;       // Startpositie van de pijl

    public GameObject crossHair;  // Verwijzing naar de crosshair

    void Start()
    {
        // Sla de startpositie van de pijl op
        startPosition = transform.position;

        // Zoek naar de crosshair object in de scene (zorg ervoor dat het een tag heeft zoals 'Crosshair')
        crossHair = GameObject.FindGameObjectWithTag("crossHair");
    }

    void Update()
    {
        // Verwijder de pijl als deze te ver van de startpositie is gegaan
        if (Vector3.Distance(transform.position, startPosition) > despawnDistance)
        {
            Destroy(gameObject);  // Verwijder de pijl als hij te ver is bewogen
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Controleer of de pijl een object met de tag "Enemy" raakt
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);  // Verwijder de pijl bij contact met een vijand
        }
    }
}
