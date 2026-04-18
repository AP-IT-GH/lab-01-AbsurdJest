using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GathererAgent : Agent
{
    private AgentActions agentActions = new AgentActions();

    [SerializeField] HunterEnvironment environment;

    [SerializeField] HunterAgent hunter;

    public float speedMultiplier = 0.1f;

    public float rotationMultiplier = 1f;

    private int hasCollected = 0;

    public override void OnEpisodeBegin()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        hasCollected = 0;
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;

        environment.SetEnvironment();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, hunter.transform.localPosition));

        float distance = 0;

        foreach (var item in environment.Treasure)
        {
            if (item.activeInHierarchy)
            {
                float newDistance = Vector3.Distance(item.transform.localPosition, this.transform.localPosition);
                if (distance == 0 || distance > newDistance)
                {
                    distance = newDistance;
                }
            }
        }

        sensor.AddObservation(distance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        agentActions.PerformActions(this, speedMultiplier, rotationMultiplier, actions);

        agentActions.AddStepRewards(this);

        AddReward(0.00001f * Vector3.Distance(this.transform.localPosition, hunter.transform.localPosition));

        Debug.Log("Gatherer: " + GetCumulativeReward());
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        agentActions.PerformHeuristicActions(actionsOut);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            hasCollected++;
            AddReward(1f);
            collision.gameObject.SetActive(false);

            if (hasCollected >= environment.treasureAmount)
            {
                AddReward(5f);
                hunter.AddReward(-5f);
                hunter.EndEpisode();
                EndEpisode();
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.05f);
        }
    }
}
