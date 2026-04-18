using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentActions
{
    public void PerformActions(Agent agent, float speedMultiplier, float rotationMultiplier, ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actions.ContinuousActions[0];
        agent.GetComponent<Rigidbody>().AddRelativeForce(controlSignal * speedMultiplier);

        Vector3 rotControlSignal = Vector3.zero;
        rotControlSignal.y = actions.ContinuousActions[1];
        agent.transform.Rotate(rotControlSignal * rotationMultiplier);
    }

    public void AddStepRewards(Agent agent)
    {
        if (agent.transform.localPosition.y < 0)
        {
            agent.AddReward(-1.0f);
            agent.EndEpisode();
        }
    }

    public void PerformHeuristicActions(ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
