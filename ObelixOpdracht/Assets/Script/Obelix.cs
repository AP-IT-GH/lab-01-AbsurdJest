using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Obelix : Agent
{
    [SerializeField] ObelixManager manager;

    public float speedMultiplier = 0.1f;

    public float rotationMultiplier = 1f;

    private bool hasRock;

    private int hasPlaced = 0;

    public override void OnEpisodeBegin()
    {
        //Reset own status
        hasRock = false;
        hasPlaced = 0;

        //Reset own position
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;

        manager.RemoveRemnants();
        manager.StartEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Own position
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.hasRock);

        float distance = 0;

        foreach(var menhir in manager.Placements)
        {
            if(menhir.tag == "Finish")
            {
                float newDistance = Vector3.Distance(menhir.transform.position, this.transform.position);
                if (distance == 0 || distance > newDistance)
                {
                    distance = newDistance;
                }
            }
        }

        sensor.AddObservation(distance);
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
            AddReward(-1.0f);
            EndEpisode();
        }

        AddReward(-0.0002f);
        Debug.Log(GetCumulativeReward());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // target bereikt
        if (collision.gameObject.CompareTag("Target") && !hasRock)
        {
            hasRock = true;
            AddReward(0.5f);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Target") && hasRock)
        {
            AddReward(-0.1f);
        }
        else if (collision.gameObject.CompareTag("Done"))
        {
            AddReward(-0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish") && hasRock)
        {
            hasPlaced++;
            hasRock = false;
            AddReward(1.0f);
            manager.SetPlaced(other.gameObject);
            if (hasPlaced == manager.AmountOfPlacements)
            {
                AddReward(1f * hasPlaced);
                EndEpisode();
            }
        }
        else if (other.gameObject.CompareTag("Finish") && !hasRock)
        {
            AddReward(-0.1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

}
