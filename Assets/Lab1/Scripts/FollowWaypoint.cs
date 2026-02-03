using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    [Header("Navigation settings")]
    public Transform[] waypoints;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 2.0f;
    public float reachDistance = 1.0f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // calculate direction to target
        Vector3 targetPosition = targetWaypoint.position;
        targetPosition.y = transform.position.y;
        Vector3 direction = targetPosition - transform.position;

        // check if we've reached the waypoint
        float distanceToWaypoint = direction.magnitude;
        if (distanceToWaypoint <= reachDistance)
        {
            // move to next waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
            return;
        }

        // Rotate towards the waypoint
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        }

        // Move forward
        float stepDistance = moveSpeed * Time.deltaTime;

        // Prevent overshooting
        if (stepDistance > distanceToWaypoint)
        {
            stepDistance = distanceToWaypoint;
        }

        transform.Translate(Vector3.forward * stepDistance);
    }
}