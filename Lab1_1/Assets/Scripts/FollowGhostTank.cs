using UnityEngine;

public class FollowGhostTank : MonoBehaviour
{
    [SerializeField] GameObject ghost;

    [SerializeField] float speed;

    [SerializeField] float rotSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = ghost.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime
        * rotSpeed);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
