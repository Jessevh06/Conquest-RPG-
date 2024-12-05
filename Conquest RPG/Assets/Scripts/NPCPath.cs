using UnityEngine;

public class NPCPath : MonoBehaviour
{
    public static Transform[] points; // Array van waypoints

    private void Awake()
    {
        // Initialiseer de waypoints array op basis van de children van dit object
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }

        if (points.Length == 0)
        {
            Debug.LogError("No waypoints found! Add child objects to the NPCPath object.");
        }
    }
}
