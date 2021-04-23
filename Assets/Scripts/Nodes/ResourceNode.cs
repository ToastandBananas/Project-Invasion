using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceType resourceType;
    public Squad laborerSquadCurrentlyOnNode;

    [HideInInspector] public List<GoldDeposit> goldDeposits;
    [HideInInspector] public List<GoldDeposit> unoccupiedDeposits;

    DefenderSpawner defenderSpawner;

    int resourceDepositCount;

    void Start()
    {
        defenderSpawner = DefenderSpawner.instance;
        resourceDepositCount = transform.childCount;

        if (resourceType == ResourceType.Gold)
        {
            for (int i = 0; i < resourceDepositCount; i++)
            {
                goldDeposits.Add(transform.GetChild(i).GetComponent<GoldDeposit>());
            }

            for (int i = 0; i < goldDeposits.Count; i++)
            {
                unoccupiedDeposits.Add(goldDeposits[i]);
            }
        }

        // Add this node to our list of nodes
        defenderSpawner.AddNode(transform.position);
        defenderSpawner.goldNodes.Add(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<Defender>(out Defender defender);

        // If a laborer enters the square
        if (defender != null && defender.squad.squadType == SquadType.Laborers && defender.isRetreating == false)
        {
            // Choose a random deposit
            int randomIndex = Random.Range(0, unoccupiedDeposits.Count);

            // Get a random x offset for the Laborer's new unit position we will be assigning it
            float coinToss = Random.Range(0, 2);
            float xOffset;
            if (coinToss == 0)
                xOffset = unoccupiedDeposits[randomIndex].miningXOffset;
            else
                xOffset = -unoccupiedDeposits[randomIndex].miningXOffset;

            // Set the Laborer's unit position to the left or right of the deposit
            defender.unitPosition = unoccupiedDeposits[randomIndex].transform.localPosition + new Vector3(xOffset, 0);

            // Set the Laborer's target deposit to this one
            if (resourceType == ResourceType.Gold)
                defender.GetComponent<Laborer>().targetGoldDeposit = unoccupiedDeposits[randomIndex];

            //Set occupied to true for the deposit
            unoccupiedDeposits[randomIndex].occupied = true;

            // Remove the deposit from our list of unoccupied deposits
            unoccupiedDeposits.Remove(unoccupiedDeposits[randomIndex]);

            laborerSquadCurrentlyOnNode = defender.squad;
        }
    }
}
