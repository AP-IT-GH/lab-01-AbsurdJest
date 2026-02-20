using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TankNav : MonoBehaviour
{
    [SerializeField] List<GameObject> waypoints;

    TMP_Dropdown wpSelector;

    NavMeshAgent agent;

    int chosenWayPoint = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wpSelector = GameObject.Find("WPSelector").GetComponent<TMP_Dropdown>();

        List<string> list = new List<string>();
        for (int i = 1; i < waypoints.Count + 1; i++)
        {
            list.Add($"Waypoint {i}");
        }
        wpSelector.AddOptions(list);

        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, waypoints[chosenWayPoint].transform.position) > 4)
        {
            agent.SetDestination(waypoints[chosenWayPoint].transform.position);
        }
    }

    public void ChangeTarget()
    {
        chosenWayPoint = wpSelector.value;
    }
}
