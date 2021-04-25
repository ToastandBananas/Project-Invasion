using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceType resourceType;

    public List<RuntimeAnimatorController> goldDepositAnimatorControllers = new List<RuntimeAnimatorController>();

    [HideInInspector] public Squad laborerSquadCurrentlyOnNode;
    [HideInInspector] public AttackerSpawner myLaneSpawner;

    [HideInInspector] public List<GoldDeposit> goldDeposits = new List<GoldDeposit>();
    [HideInInspector] public List<GoldDeposit> unoccupiedDeposits = new List<GoldDeposit>();
    // [HideInInspector] public List<Attacker> attackersAttackingNode = new List<Attacker>();

    DefenderSpawner defenderSpawner;

    int resourceDepositCount;

    void Start()
    {
        SetLaneSpawner();

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

        // Choose a random sprite
        for (int i = 0; i < resourceDepositCount; i++)
        {
            if (resourceType == ResourceType.Gold)
            {
                int randomIndex = Random.Range(0, goldDepositAnimatorControllers.Count);
                goldDeposits[i].anim.runtimeAnimatorController = goldDepositAnimatorControllers[randomIndex];
                goldDepositAnimatorControllers.Remove(goldDepositAnimatorControllers[randomIndex]);
            }

            int coinToss = Random.Range(0, 2);
            if (coinToss == 0)
                goldDeposits[i].sr.flipX = false;
            else
                goldDeposits[i].sr.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Attacker attacker);
        collision.TryGetComponent(out Defender defender);

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
            {
                defender.GetComponent<Laborer>().targetGoldDeposit = unoccupiedDeposits[randomIndex];
                unoccupiedDeposits[randomIndex].myLaborer = defender;
            }

            //Set occupied to true for the deposit
            unoccupiedDeposits[randomIndex].occupied = true;

            // Remove the deposit from our list of unoccupied deposits
            unoccupiedDeposits.Remove(unoccupiedDeposits[randomIndex]);

            laborerSquadCurrentlyOnNode = defender.squad;
        }
        else if (attacker != null && attacker.canAttackNodes) // If an attacker who can attack resource nodes enters the square
        {
            // attackersAttackingNode.Add(attacker);

            // Set the Attacker's target Resource Node to this Node
            attacker.currentTargetNode = this;

            // Set the Attacker's target resource deposit to a random deposit within this Node
            AssignTargetToAttacker(attacker);
        }
    }

    public void AssignTargetToAttacker(Attacker attacker)
    {
        if (resourceType == ResourceType.Gold && goldDeposits.Count > 0)
            attacker.currentTargetGoldDeposit = goldDeposits[Random.Range(0, goldDeposits.Count)];
        else
            attacker.ClearTargetVariables();
    }

    void SetLaneSpawner()
    {
        AttackerSpawner[] attackerSpawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            bool isCloseEnough = (Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon);

            if (isCloseEnough) myLaneSpawner = spawner;
        }
    }

    /*void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out Attacker attacker);
        if (attacker != null && attackersAttackingNode.Contains(attacker))
            attackersAttackingNode.Remove(attacker);
    }*/
}
