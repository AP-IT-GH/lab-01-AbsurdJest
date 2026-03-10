using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveTank : MonoBehaviour
{
    [SerializeField] WPManager wpManager;

    Graph graph;

    TMP_Dropdown wpSelector;

    GameObject currentNode;

    GameObject endNode;

    int chosenWaypoint;

    int nextWaypoint = 0;

    int currentWaypoint;

    [SerializeField] float speed;

    [SerializeField] float rotSpeed;

    bool hasStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        graph = wpManager.graph;

        wpSelector = GameObject.Find("WPSelector").GetComponent<TMP_Dropdown>();

        List<string> list = new List<string>();
        for (int i = 1; i < wpManager.waypoints.Length +1; i++)
        {
            list.Add($"Waypoint {i}");
        }
        wpSelector.AddOptions(list);
        currentNode = wpManager.waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted)
        {
            int calcWaypoint = currentWaypoint + nextWaypoint;

            if (calcWaypoint >= wpManager.waypoints.Length)
                calcWaypoint -= wpManager.waypoints.Length;
            if (Vector3.Distance(gameObject.transform.position, wpManager.waypoints[calcWaypoint].transform.position) > 4)
            {
                Vector3 direction = wpManager.waypoints[calcWaypoint].transform.position - transform.position;

                Quaternion lookRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else
            {
                currentWaypoint = nextWaypoint;
                currentNode = wpManager.waypoints[currentWaypoint];
                nextWaypoint++;
                if(currentWaypoint == chosenWaypoint)
                {
                    hasStarted = false;
                }
            }
        }
    }

    public void ChangeTarget()
    {
        nextWaypoint = 0;
        chosenWaypoint = wpSelector.value;
        endNode = wpManager.waypoints[chosenWaypoint];
        graph.AStar(currentNode, endNode);
        hasStarted = true;
    }
}
