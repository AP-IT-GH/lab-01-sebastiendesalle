using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingController : MonoBehaviour
{
    [Header("References")]
    public WPManager wpManager;
    public GameObject tank;
    public TMP_Dropdown nodeDropdown;

    private FollowNodes tankFollowScript;

    void Start()
    {
        tankFollowScript = tank.GetComponent<FollowNodes>();

        // Populate dropdown with waypoint names
        PopulateDropdown();

        // Add listener for dropdown changes
        nodeDropdown.onValueChanged.AddListener(OnNodeSelected);
    }

    void PopulateDropdown()
    {
        nodeDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < wpManager.waypoints.Length; i++)
        {
            options.Add("Node " + i + " (" + wpManager.waypoints[i].name + ")");
        }

        nodeDropdown.AddOptions(options);
    }

    void OnNodeSelected(int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= wpManager.waypoints.Length)
            return;

        GameObject targetNode = wpManager.waypoints[selectedIndex];

        // Find the closest waypoint to the tank's current position
        GameObject closestNode = FindClosestWaypoint();

        // Calculate path using A star
        bool pathFound = wpManager.graph.AStar(closestNode, targetNode);

        if (pathFound)
        {
            // Convert the path to Transform array for FollowNodes
            Transform[] pathTransforms = new Transform[wpManager.graph.pathList.Count];

            for (int i = 0; i < wpManager.graph.pathList.Count; i++)
            {
                pathTransforms[i] = wpManager.graph.getPathPoint(i).transform;
            }

            // Update the tanks waypoints
            tankFollowScript.waypoints = pathTransforms;
            tankFollowScript.ResetToFirstWaypoint();

            Debug.Log("Path found, going to " + pathTransforms.Length + " waypoints to " + targetNode.name);
        }
        else
        {
            Debug.LogWarning("No path found to " + targetNode.name);
        }
    }

    GameObject FindClosestWaypoint()
    {
        GameObject closest = wpManager.waypoints[0];
        float closestDistance = Vector3.Distance(tank.transform.position, closest.transform.position);

        for (int i = 1; i < wpManager.waypoints.Length; i++)
        {
            float distance = Vector3.Distance(tank.transform.position, wpManager.waypoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = wpManager.waypoints[i];
            }
        }

        return closest;
    }
}