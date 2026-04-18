using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class HunterAgent : Agent
{
    private AgentActions agentActions = new AgentActions();

    [SerializeField] GathererAgent gatherer;

    public float speedMultiplier = 0.1f;

    public float rotationMultiplier = 1f;

    public override void OnEpisodeBegin()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        float x = 8 * Mathf.Cos(2 * Mathf.PI * Random.Range(1, 8) / 8);
        float z = 8 * Mathf.Sin(2 * Mathf.PI * Random.Range(1, 8) / 8);

        this.transform.localPosition = new Vector3(x, 0.5f, z);
        this.transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, gatherer.transform.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        agentActions.PerformActions(this, speedMultiplier, rotationMultiplier, actions);

        agentActions.AddStepRewards(this);

        AddReward(-0.000005f * Vector3.Distance(this.transform.localPosition, gatherer.transform.localPosition));

        Debug.Log("Hunter: " + GetCumulativeReward());
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        agentActions.PerformHeuristicActions(actionsOut);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gatherer"))
        {
            AddReward(5f);
            gatherer.AddReward(-5f);
            gatherer.EndEpisode();
            EndEpisode();
        }
        else {
            AddReward(-0.05f);
        }
    }
}
