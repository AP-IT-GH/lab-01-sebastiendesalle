using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Navigation settings")]
    public Transform[] waypoints;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 2.0f;
    public float reachDistance = 1.0f;
    private int currentWaypointIndex = 0;
    void Start()
    {

    }

    void Update()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) > reachDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}
