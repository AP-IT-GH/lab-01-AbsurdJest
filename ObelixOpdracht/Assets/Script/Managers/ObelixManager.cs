using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObelixManager : MonoBehaviour
{
    public List<GameObject> Placements = new List<GameObject>();

    private List<GameObject> rocks = new List<GameObject>();

    public GameObject PlacementPrefab;

    public GameObject RockPrefab;

    public int AmountOfPlacements;

    [SerializeField] int size;

    [SerializeField] Material red;

    [SerializeField] Material green;

    private bool hasStarted = false;

    public void RemoveRemnants()
    {
        for (int i = 0; i < Placements.Count; i++)
        {
            SetNotPlaced(Placements[i]);
        }

        for (int i = 0; i < rocks.Count; i++)
        {
            Destroy(rocks[i]);
        }
    }

    public void StartEpisode()
    {
        for (int i = 0; i < AmountOfPlacements; i++)
        {
            if (!hasStarted)
            {
                float x = size * Mathf.Cos(2 * Mathf.PI * i / AmountOfPlacements);
                float z = size * Mathf.Sin(2 * Mathf.PI * i / AmountOfPlacements);

                var zone = Instantiate(PlacementPrefab, transform.parent);
                zone.transform.localPosition = new Vector3(x, 1.5f, z);
                Placements.Add(zone);
            }

            var target = Instantiate(RockPrefab, transform.parent);
            target.transform.localPosition = new Vector3(Random.value * 28 - 14, 1.5f, Random.value * 28 - 14);
            rocks.Add(target);
        }
        hasStarted = true;
    }

    public void SetPlaced(GameObject placement)
    {
        placement.gameObject.tag = "Done";
        placement.gameObject.GetComponent<Renderer>().material = green;
        placement.gameObject.GetComponent<Collider>().isTrigger = false;
    }

    private void SetNotPlaced(GameObject placement)
    {
        placement.tag = "Finish";
        placement.GetComponent<Renderer>().material = red;
        placement.GetComponent<Collider>().isTrigger = true;
    }
}
