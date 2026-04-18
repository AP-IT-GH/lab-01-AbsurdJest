using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HunterEnvironment : MonoBehaviour
{
    public List<GameObject> Treasure = new List<GameObject>();

    [SerializeField] GameObject treasurePrefab;

    private bool hasGenerated;

    public int treasureAmount;

    private void GenerateTreasures()
    {
        for (int i = 0; i < treasureAmount; i++)
        {
            var treasurePiece = Instantiate(treasurePrefab, transform.parent);
            Treasure.Add(treasurePiece);
        }
    }

    public void SetEnvironment()
    {
        if (!hasGenerated)
        {
            GenerateTreasures();
            hasGenerated = true;
        }

        foreach (var treasurePiece in Treasure)
        {
            treasurePiece.transform.localPosition = new Vector3(Random.value * 16 - 8, 0.3f, Random.value * 16 - 8);
            treasurePiece.gameObject.SetActive(true);
        }
    }
}
