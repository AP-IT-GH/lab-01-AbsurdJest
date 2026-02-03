using UnityEngine;

public class FollowWayPoint : MonoBehaviour
{
    private int currentWaypoint;

    [SerializeField] GameObject[] waypoints;

    [SerializeField] float speed;

    [SerializeField] float rotSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = waypoints[currentWaypoint].transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime
        * rotSpeed);

        transform.Translate(0, 0, speed * Time.deltaTime);

        DistanceCheck();
    }

    void DistanceCheck()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position) < 2)
        {
            currentWaypoint += 1;
            if (currentWaypoint > waypoints.Length - 1)
                currentWaypoint = 0;
        }
    }
}
