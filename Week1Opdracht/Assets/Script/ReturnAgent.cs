using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ReturnAgent : Agent
{
    public Transform Target;

    public float speedMultiplier = 0.1f;

    public float rotationMultiplier = 1f;

    private int timer = 0;

    private bool hasTarget = false;

    public override void OnEpisodeBegin()
    {
        Target.gameObject.SetActive(true);
        hasTarget = false;
        timer = 0;
        //Use localposition
        if (this.transform.localPosition.y < 0)
        {
            //Reset own position
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }


        //Set Target location to random location
        Target.localPosition = new Vector3(Random.value * 6 - 2, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        // Own position
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);

        Vector3 rotControlSignal = Vector3.zero;
        rotControlSignal.y = actionBuffers.ContinuousActions[1];
        transform.Rotate(rotControlSignal * rotationMultiplier);


        // Van het platform gevallen?
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
            AddReward(-1.0f);
        }
        timer++;

        AddReward(-0.00005f * timer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // target bereikt
        if (collision.gameObject.CompareTag("Target"))
        {
            AddReward(5.0f);
            collision.gameObject.SetActive(false);
            hasTarget = true;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Finish") && hasTarget)
        {
            AddReward(5.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}

